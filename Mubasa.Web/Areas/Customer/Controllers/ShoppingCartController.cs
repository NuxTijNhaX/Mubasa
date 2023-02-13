using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility;
using Mubasa.Web.Services.ThirdParties.Carrier.GiaoHangNhanh;
using Mubasa.Web.Services.ThirdParties.PaymentGateway.MoMo;
using Mubasa.Web.Services.ThirdParties.PaymentGateway.ZaloPay;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IOptions<GiaoHangNhanhConfig> _ghnConfig;
        private readonly IOptions<ZaloPayConfig> _zaloPayConfig;
        private readonly IOptions<MoMoConfig> _momoConfig;

        public CheckoutVM CheckoutVM { get; set; }

        public ShoppingCartController(
            IUnitOfWork db, 
            IStringLocalizer<HomeController> localizer, 
            IOptions<GiaoHangNhanhConfig> ghnConfig,
            IOptions<ZaloPayConfig> zaloPayConfig,
            IOptions<MoMoConfig> momoConfig)
        {
            _db = db;
            _localizer = localizer;
            _ghnConfig = ghnConfig;
            _zaloPayConfig = zaloPayConfig;
            _momoConfig = momoConfig;
        }

        // GET: ShoppingCartController
        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            CheckoutVM = new CheckoutVM()
            {
                ShoppingCartVM = new(),
            };

            CheckoutVM.ShoppingCartVM.ShoppingCart = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == claim.Value, includeProp: "Product");

            var defaultAddressId = _db.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value).AddressId;
            if(defaultAddressId != null)
            {
                CheckoutVM.ShoppingCartVM.Address = _db.Address.GetFirstOrDefault(i => i.Id == defaultAddressId, "Province,District,Ward");
                CheckoutVM.ShoppingCartVM.Address.FullAddress = $"{CheckoutVM.ShoppingCartVM.Address.HomeNumber}, {CheckoutVM.ShoppingCartVM.Address.Ward.Name}, {CheckoutVM.ShoppingCartVM.Address.District.Name}, {CheckoutVM.ShoppingCartVM.Address.Province.Name}";
            }
            

            foreach (var item in CheckoutVM.ShoppingCartVM.ShoppingCart)
            {
                item.SubTotal = item.Count * item.Product.Price;
                CheckoutVM.ShoppingCartVM.SubTotal += item.SubTotal;
            }

            return View(CheckoutVM.ShoppingCartVM);
        }

        public async Task<IActionResult> CheckOut()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var defaultAddressId = _db.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value).AddressId;

            CheckoutVM = new CheckoutVM()
            {
                ShoppingCartVM = new(),
                OrderHeader = new(),
                PaymentMethods = _db.PaymentMethod.GetAll(),
                ShippingMethods = new List<ShippingMethod>(),
            };

            CheckoutVM.ShoppingCartVM.Address = _db.Address.GetFirstOrDefault(i => i.Id == defaultAddressId, "Province,District,Ward");
            CheckoutVM.ShoppingCartVM.Address.FullAddress = $"{CheckoutVM.ShoppingCartVM.Address.HomeNumber}, {CheckoutVM.ShoppingCartVM.Address.Ward.Name}, {CheckoutVM.ShoppingCartVM.Address.District.Name}, {CheckoutVM.ShoppingCartVM.Address.Province.Name}";
            
            CheckoutVM.ShoppingCartVM.ShoppingCart = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == claim.Value, includeProp: "Product");
            if(CheckoutVM.ShoppingCartVM.ShoppingCart.Count() == 0)
            {
                TempData["info"] = "Giỏ hàng trống!\nThêm hàng vào giỏ ngay thôi!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                CheckoutVM.ShippingMethods = await GetShippingMethods(CheckoutVM.ShoppingCartVM.Address.District.Id_Ghn, CheckoutVM.ShoppingCartVM.Address.Ward.Id_Ghn);
            }
            catch
            {
                CheckoutVM.ShippingMethods = new List<ShippingMethod>()
                {
                    new ShippingMethod()
                    {
                        ServiceId = "0",
                        ServiceTypeId= "0",
                        Name = "Mubasa.com vận chuyển",
                        Price = 50000,
                        ExpectedDay = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy"),
                    },
                };
            }

            foreach (var item in CheckoutVM.ShoppingCartVM.ShoppingCart)
            {
                item.SubTotal = item.Count * item.Product.Price;
                CheckoutVM.ShoppingCartVM.SubTotal += item.SubTotal;
            }

            CheckoutVM.OrderHeader.ApplicationUserId = claim.Value;
            CheckoutVM.OrderHeader.ReceiverName = CheckoutVM.ShoppingCartVM.Address.ReceiverName;
            CheckoutVM.OrderHeader.ReceiverPhoneNumber = CheckoutVM.ShoppingCartVM.Address.PhoneNumber;
            CheckoutVM.OrderHeader.ShippingAddress = CheckoutVM.ShoppingCartVM.Address.HomeNumber;
            CheckoutVM.OrderHeader.WardId = CheckoutVM.ShoppingCartVM.Address.WardId;
            CheckoutVM.OrderHeader.DistrictId = CheckoutVM.ShoppingCartVM.Address.DistrictId;
            CheckoutVM.OrderHeader.ProvinceId = CheckoutVM.ShoppingCartVM.Address.ProvinceId;

            if (!(TempData.ContainsKey("ServiceId") && TempData.ContainsKey("ServiceTypeId")))
            {
                var firstShipMethod = CheckoutVM.ShippingMethods.FirstOrDefault();
                TempData["ServiceId"] = firstShipMethod.ServiceId;
                TempData["ServiceTypeId"] = firstShipMethod.ServiceTypeId;

                CheckoutVM.OrderHeader.ShippingCost = CheckoutVM.ShippingMethods.First().Price;
            }
            else
            {
                string serviceId = TempData["ServiceId"].ToString();
                string serviceTypeId = TempData["ServiceTypeId"].ToString();

                var selectedShipMethod = CheckoutVM.ShippingMethods.FirstOrDefault(i => i.ServiceId == serviceId && i.ServiceTypeId == serviceTypeId);
                CheckoutVM.OrderHeader.ShippingCost = selectedShipMethod.Price;
            }

            CheckoutVM.OrderHeader.GrandTotal = CheckoutVM.ShoppingCartVM.SubTotal + CheckoutVM.OrderHeader.ShippingCost - CheckoutVM.OrderHeader.Discount;

            if (!TempData.ContainsKey("PaymentId"))
            {
                var firstPaymentMethod = CheckoutVM.PaymentMethods.First();
                TempData["PaymentId"] = firstPaymentMethod.Id;
                CheckoutVM.OrderHeader.PaymentMethodId = int.Parse(TempData["PaymentId"].ToString());
            }
            else
            {
                int paymentId = int.Parse(TempData["PaymentId"].ToString());
                CheckoutVM.OrderHeader.PaymentMethodId = paymentId;
            }

            return View(CheckoutVM);
        }

        [HttpPost]
        [ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutPost(CheckoutVM checkoutVM)
        {
            #region Initial
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            checkoutVM.OrderHeader = new();
            
            var address = _db.Address.GetFirstOrDefault(i => i.Id == checkoutVM.ShoppingCartVM.Address.Id, "Province,District,Ward");

            int paymentId = int.Parse(TempData["PaymentId"].ToString());
            PaymentMethod paymentMethod = _db.PaymentMethod.GetFirstOrDefault(i => i.Id == paymentId);

            checkoutVM.ShoppingCartVM.ShoppingCart = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == claim.Value, includeProp: "Product");
            foreach (var item in checkoutVM.ShoppingCartVM.ShoppingCart)
            {
                item.SubTotal = item.Count * item.Product.Price;
                checkoutVM.ShoppingCartVM.SubTotal += item.SubTotal;
            }

            checkoutVM.ShippingMethods = await GetShippingMethods(address.District.Id_Ghn, address.Ward.Id_Ghn);
            string serviceId = TempData["ServiceId"].ToString();
            string serviceTypeId = TempData["ServiceTypeId"].ToString();
            var selectedShipMethod = checkoutVM.ShippingMethods.FirstOrDefault(i => i.ServiceId == serviceId && i.ServiceTypeId == serviceTypeId);

            #endregion

            #region OrderHeader

            checkoutVM.OrderHeader.ShippingCost = selectedShipMethod.Price;
            checkoutVM.OrderHeader.ApplicationUserId = claim.Value;
            checkoutVM.OrderHeader.ReceiverName = address.ReceiverName;
            checkoutVM.OrderHeader.ReceiverPhoneNumber = address.PhoneNumber;
            checkoutVM.OrderHeader.ShippingAddress = address.HomeNumber;
            checkoutVM.OrderHeader.WardId = address.WardId;
            checkoutVM.OrderHeader.DistrictId = address.DistrictId;
            checkoutVM.OrderHeader.ProvinceId = address.ProvinceId;
            checkoutVM.OrderHeader.PaymentMethodId = paymentId;
            checkoutVM.OrderHeader.GrandTotal = checkoutVM.ShoppingCartVM.SubTotal + checkoutVM.OrderHeader.ShippingCost - checkoutVM.OrderHeader.Discount;
            checkoutVM.OrderHeader.OrderStatus = SD.OrderPending;
            checkoutVM.OrderHeader.PaymentStatus = SD.PaymentWaiting;
            checkoutVM.OrderHeader.ShippingInfo = JsonConvert.SerializeObject(selectedShipMethod);
            checkoutVM.OrderHeader.CreatedDate = DateTime.Now;

            _db.OrderHeader.Add(checkoutVM.OrderHeader);
            _db.Save();

            #endregion

            #region OrderDetail

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in checkoutVM.ShoppingCartVM.ShoppingCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = item.Product.Price,
                    OrderHeaderId = checkoutVM.OrderHeader.Id,
                };

                orderDetails.Add(orderDetail);
            }

            _db.OrderDetail.AddRange(orderDetails);
            _db.Save();

            #endregion


            if (paymentMethod.Code == SD.PayMethod_COD)
            {
                DeleteShoppingCart(checkoutVM.OrderHeader.ApplicationUserId);
            }
            else
            {
                checkoutVM.OrderHeader.OrderStatus = SD.OrderWait4Pay;

                if (paymentMethod.Code == SD.PayMethod_Zalo)
                {
                    HandleZaloPayment(
                        checkoutVM.OrderHeader.Id,
                        checkoutVM.ShoppingCartVM.ShoppingCart,
                        checkoutVM.OrderHeader.GrandTotal);
                }
                else if (paymentMethod.Code == SD.PayMethod_MoMo)
                {
                    MoMo _momo = new(_momoConfig);
                    var momoResponse = await _momo
                        .CreateOrder(
                            checkoutVM.OrderHeader.Id,
                            paymentMethod.Code,
                            checkoutVM.OrderHeader.ApplicationUserId,
                            checkoutVM.OrderHeader.GrandTotal
                        );

                    var resultCode = int.Parse(momoResponse["resultCode"]);

                    if (resultCode == 0)
                    {
                        var payUrl = momoResponse["payUrl"];

                        DeleteShoppingCart(checkoutVM.OrderHeader.ApplicationUserId);
                        _db.Save();

                        return Redirect(payUrl);
                    }
                }

            }

            return RedirectToAction("Index", "Home");
        }

        // POST: ShoppingCartController/Delete/5
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var shoppingItem = _db.ShoppingItem.GetFirstOrDefault(i => i.ProductId == id && claim.Value == i.ApplicationUserId);

                if (shoppingItem == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.ShoppingItem.Remove(shoppingItem);
                _db.Save();

                HttpContext.Session.SetInt32(
                    SD.SessionCart,
                    _db.ShoppingItem
                        .GetAll(i => i.ApplicationUserId == claim.Value)
                        .ToList()
                        .Count);

                return Json(new { success = true, message = $"{_localizer["Delete Successful"]}" });
            }
            catch
            {
                return Json(new { success = false, message = $"{_localizer["Error Deleting Data"]}" });
            }
        }

        public IActionResult PaymentResult(
            int orderId,
            string paymentName,
            string userId,
            int resultCode)
        {

            if(resultCode == 0)
            {
                UpdatePaidOrder(orderId, paymentName, userId);
                return View(true);
            }

            return View(false);
        }

        public IActionResult UpdateShippingMethod(
            string serviceId, 
            string serviceTypeId)
        {
            TempData["ServiceId"] = serviceId;
            TempData["ServiceTypeId"] = serviceTypeId;

            return RedirectToAction(nameof(CheckOut));
        }

        public IActionResult UpdatePaymentMethod(int id)
        {
            var paymentCode = _db.PaymentMethod.GetFirstOrDefault(i => i.Id == id).Code;

            if (paymentCode == SD.PayMethod_Zalo)
            {
                TempData["failure"] = "Ví Zalo đang bảo trì";
            }
            else
            {
                TempData["PaymentId"] = id;
            }

            return RedirectToAction(nameof(CheckOut));
        }

        public IActionResult IncreCount(int shoppingItemId)
        {
            var shoppingItem = _db.ShoppingItem.GetFirstOrDefault(i => shoppingItemId == i.Id);
            if (shoppingItem != null)
            {
                _db.ShoppingItem.IncreQuantity(shoppingItem, 1);
                _db.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecreCount(int shoppingItemId)
        {
            var shoppingItem = _db.ShoppingItem.GetFirstOrDefault(i => shoppingItemId == i.Id);
            if (shoppingItem != null)
            {
                if(shoppingItem.Count == 1) return RedirectToAction(nameof(Index));

                _db.ShoppingItem.DecreQuantity(shoppingItem, 1);
                _db.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<ShippingMethod>> GetShippingMethods(int districtId, string ward_id)
        {
            try
            {
                GiaoHangNhanh GHN = new(_ghnConfig);

                string district_id = districtId.ToString();

                List<ShippingMethod> shippingMethods = new List<ShippingMethod>();

                var shippingService = await GHN.GetService(district_id);

                if(shippingService != null )
                {
                    foreach (var item in shippingService)
                    {
                        string serviceId = item.service_id;
                        string serviceTypeId = item.service_type_id;
                        var leadTime = await GHN.GetLeadTime(district_id, ward_id, serviceId);
                        var fee = await GHN.GetFee(district_id, ward_id, serviceId, serviceTypeId);
                        
                        ShippingMethod method = new()
                        {
                            ServiceId = serviceId,
                            ServiceTypeId = serviceTypeId,
                            Name = item.short_name,
                            ExpectedDay = leadTime,
                            Price = Double.Parse(fee),
                        };

                        shippingMethods.Add(method);
                    }
                }

                return shippingMethods;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdatePaidOrder(int orderId, string paymentMethodName, string appuserId)
        {
            var orderHeader = _db.OrderHeader.GetFirstOrDefault(i => i.Id == orderId);
            // orderHeader.PartnerPaymentId = paymentMethodName;
            orderHeader.PaymentStatus = SD.PaymentPaid;
            orderHeader.PaymentedDate = DateTime.Now;

            DeleteShoppingCart(appuserId);

            _db.Save();
        }
    
        private async Task<IActionResult> HandleZaloPayment(int orderId, IEnumerable<ShoppingItem> shoppingCart, double amount)
        {
            ZaloPay zaloPay = new(_zaloPayConfig);

            var zalopayResponse = await zaloPay
                .CreateOrder(
                    shoppingCart,
                    orderId,
                    amount
                );

            var returncode = int.Parse(zalopayResponse["returncode"]);

            if (returncode == 1)
            {
                var orderurl = zalopayResponse["orderurl"];
                return Redirect(orderurl);
            }

            return NotFound();
        }
    
        private void DeleteShoppingCart(string appuserId)
        {
            var shoppingCart = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == appuserId);
            _db.ShoppingItem.RemoveRange(shoppingCart);

            HttpContext.Session.Clear();

            _db.Save();
        }
    }
}
