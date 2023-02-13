using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility;
using Mubasa.Web.Services.ThirdParties.PaymentGateway.MoMo;
using System.Security.Claims;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerOrderController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IOptions<MoMoConfig> _momoConfig;
        public CustomerOrderController(
            IUnitOfWork db, 
            IOptions<MoMoConfig> momoConfig)
        {
            _db = db;
            _momoConfig = momoConfig;
        }

        // GET: CustomerOrderController
        public ActionResult Index(string? status = SD.OrderWait4Pay)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orders;

            if (status == SD.OrderAll)
            {
                orders = _db.OrderHeader.GetAll(
                    i => i.ApplicationUserId == claim.Value);
            }
            else
            {
                orders = _db.OrderHeader.GetAll(
                    i => i.OrderStatus == status && i.ApplicationUserId == claim.Value);
            }

            return View(orders);
        }

        // GET: CustomerOrderController/Details/5
        public ActionResult Details(int id)
        {
            OrderDetailVM orderDetailVM = new()
            {
                OrderHeader = _db.OrderHeader.GetFirstOrDefault(i => i.Id == id, includeProp: "Ward,District,Province,PaymentMethod"),
                OrderDetails = _db.OrderDetail.GetAll(i => i.OrderHeaderId == id, includeProp: "Product"),
            };

            foreach (var item in orderDetailVM.OrderDetails)
            {
                var price = item.Quantity * item.UnitPrice;
                orderDetailVM.SubTotal += price;
            }

            return View(orderDetailVM);
        }

        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            var order = _db.OrderHeader.GetFirstOrDefault(i => i.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            if(order.PaymentStatus == SD.PaymentPaid)
            {
                // TODO: return Money
            }

            order.OrderStatus = status;
            _db.Save();

            TempData["success"] = "Hủy đơn hàng thành công";
            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        public async Task<IActionResult> PayOrder(int orderId)
        {
            var order = _db.OrderHeader.GetFirstOrDefault(
                i => i.Id == orderId, 
                includeProp: "PaymentMethod");

            var paymentCode = order.PaymentMethod.Code;

            if (order == null)
            {
                return NotFound();
            }

            if (paymentCode == SD.PayMethod_Zalo)
            {
                // TODO
            }
            else if (paymentCode == SD.PayMethod_MoMo)
            {
                // TODO
            }

            TempData["failure"] = "Lỗi thanh toán";

            return RedirectToAction("Index");
        }
    }
}
