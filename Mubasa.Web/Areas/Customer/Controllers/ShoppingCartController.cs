using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility.ThirdParties.Carrier;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using NuGet.Protocol;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IOptions<GiaoHangNhanh> _ghn;

        public ShoppingCartController(
            IUnitOfWork db, 
            IStringLocalizer<HomeController> localizer, 
            IOptions<GiaoHangNhanh> ghn)
        {
            _db = db;
            _localizer = localizer;
            _ghn = ghn;
        }

        // GET: ShoppingCartController
        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var defaultAddressId = _db.DefaultAddress.GetFirstOrDefault(i => i.ApplicationUserId == claim.Value).AddressId;

            ShoppingCartVM shoppingCart = new ShoppingCartVM()
            {
                ShoppingCarts = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == claim.Value, includeProp: "Product"),
                Address = _db.Address.GetFirstOrDefault(i => i.Id == defaultAddressId, "Province,District,Ward"),
            };

            shoppingCart.Address.FullAddress = $"{shoppingCart.Address.HomeNumber}, {shoppingCart.Address.Ward.Name}, {shoppingCart.Address.District.Name}, {shoppingCart.Address.Province.Name}";

            foreach (var item in shoppingCart.ShoppingCarts)
            {
                item.SubTotal = item.Count * item.Product.Price;
                shoppingCart.SubTotal += item.SubTotal;
            }

            return View(shoppingCart);
        }

        public async Task<IActionResult> CheckOut()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var defaultAddressId = _db.DefaultAddress.GetFirstOrDefault(i => i.ApplicationUserId == claim.Value).AddressId;

            CheckoutVM checkoutVM = new CheckoutVM()
            {
                ShoppingCarts = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == claim.Value, includeProp: "Product"),
                Address = _db.Address.GetFirstOrDefault(i => i.Id == defaultAddressId, "Province,District,Ward"),
                OrderHeader = new(),
                PaymentMethods = _db.PaymentMethod.GetAll(),
                ShippingMethods = new List<ShippingMethod>(),
            };

            checkoutVM.ShippingMethods = await GetShippingMethods(checkoutVM.Address.District.Id_Ghn, checkoutVM.Address.Ward.Id_Ghn);

            checkoutVM.Address.FullAddress = $"{checkoutVM.Address.HomeNumber}, {checkoutVM.Address.Ward.Name}, {checkoutVM.Address.District.Name}, {checkoutVM.Address.Province.Name}";
            checkoutVM.OrderHeader.ReceiverName = checkoutVM.Address.ReceiverName;
            checkoutVM.OrderHeader.ReceiverPhoneNumber = checkoutVM.Address.PhoneNumber;
            checkoutVM.OrderHeader.WardId = checkoutVM.Address.WardId;
            checkoutVM.OrderHeader.DistrictId = checkoutVM.Address.DistrictId;
            checkoutVM.OrderHeader.ProvinceId = checkoutVM.Address.ProvinceId;

            if (!(TempData.ContainsKey("ServiceId") && TempData.ContainsKey("ServiceTypeId")))
            {
                var firstShipMethod = checkoutVM.ShippingMethods.First();
                TempData["ServiceId"] = firstShipMethod.ServiceId;
                TempData["ServiceTypeId"] = firstShipMethod.ServiceTypeId;

                checkoutVM.OrderHeader.ShippingCost = checkoutVM.ShippingMethods.First().Price;
            }
            else
            {
                string serviceId = TempData["ServiceId"].ToString();
                string serviceTypeId = TempData["ServiceTypeId"].ToString();

                var selectedShipMethod = checkoutVM.ShippingMethods.FirstOrDefault(i => i.ServiceId == serviceId && i.ServiceTypeId == serviceTypeId);
                checkoutVM.OrderHeader.ShippingCost = selectedShipMethod.Price;
            }

            foreach (var item in checkoutVM.ShoppingCarts)
            {
                item.SubTotal = item.Count * item.Product.Price;
                checkoutVM.SubTotal += item.SubTotal;
            }

            checkoutVM.OrderHeader.GrandTotal = checkoutVM.SubTotal + checkoutVM.OrderHeader.ShippingCost - checkoutVM.OrderHeader.Discount;

            return View(checkoutVM);
        }

        public IActionResult UpdateShippingMethod(string serviceId, string serviceTypeId)
        {
            TempData["ServiceId"] = serviceId;
            TempData["ServiceTypeId"] = serviceTypeId;

            return RedirectToAction(nameof(CheckOut));
        }

        [HttpPost]
        public IActionResult Checkout()
        {

            return View();
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

                var shoppingCart = _db.ShoppingItem.GetFirstOrDefault(i => i.ProductId == id && claim.Value == i.ApplicationUserId);

                if (shoppingCart == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.ShoppingItem.Remove(shoppingCart);
                _db.Save();

                return Json(new { success = true, message = $"{_localizer["Delete Successful"]}" });
            }
            catch
            {
                return Json(new { success = false, message = $"{_localizer["Error Deleting Data"]}" });
            }
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
                GiaoHangNhanh GHN = new GiaoHangNhanh()
                {
                    EndPoint = _ghn.Value.EndPoint,
                    Token = _ghn.Value.Token,
                    ShopId = _ghn.Value.ShopId,
                    DistrictId = _ghn.Value.DistrictId,
                    WardId = _ghn.Value.WardId,
                };

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

    }
}
