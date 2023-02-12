using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility;
using System.Security.Claims;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerOrderController : Controller
    {
        private readonly IUnitOfWork _db;
        public CustomerOrderController(IUnitOfWork db)
        {
            _db = db;
        }

        // GET: CustomerOrderController
        public ActionResult Index(string? status = SD.OrderPending)
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

            return View(orderDetailVM);
        }

        // GET: CustomerOrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerOrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerOrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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

        public IActionResult PayOrder(int orderId)
        {
            //var order = _db.OrderHeader.GetFirstOrDefault(i => i.Id == orderId);

            //if (order == null)
            //{
            //    return NotFound();
            //}

            //if (order.PaymentStatus == SD.PaymentPaid)
            //{
            //    // TODO: return Money
            //}

            //order.OrderStatus = status;
            //_db.Save();

            //TempData["success"] = "Hủy đơn hàng thành công";
            //return RedirectToAction(nameof(Details), new { id = orderId });

            return RedirectToAction("Index");
        }
    }
}
