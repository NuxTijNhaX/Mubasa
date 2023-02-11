using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _db;
        public OrderController(IUnitOfWork db)
        {
            _db = db;
        }

        // GET: OrderController
        public IActionResult Index(string? status = SD.OrderPending)
        {
            IEnumerable<OrderHeader> orders;

            if (status == SD.OrderAll)
            {
                orders = _db.OrderHeader.GetAll(includeProp: "Province");
            }
            else
            {
                orders = _db.OrderHeader.GetAll(
                    i => i.OrderStatus == status, 
                    includeProp: "Province");
            }

            return View(orders);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            OrderDetailVM orderDetailVM = new()
            {
                OrderHeader = _db.OrderHeader.GetFirstOrDefault(i => i.Id == id, includeProp: "Ward,District,Province,PaymentMethod"),
                OrderDetails = _db.OrderDetail.GetAll(i => i.OrderHeaderId == id, includeProp: "Product"),
            };

            return View(orderDetailVM);
        }

        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            var order = _db.OrderHeader.GetFirstOrDefault(i => i.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = status;
            _db.Save();

            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction(nameof(Details), new {id = orderId});
        }
    }
}
