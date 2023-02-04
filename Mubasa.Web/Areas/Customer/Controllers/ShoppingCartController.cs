using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using System.Security.Claims;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        public ShoppingCartController(IUnitOfWork db, IStringLocalizer<HomeController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        // GET: ShoppingCartController
        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingCart = new ShoppingCartVM()
            {
                ShoppingCarts = _db.ShoppingItem.GetAll(i => i.ApplicationUserId == claim.Value, includeProp: "Product")
            };

            foreach (var item in shoppingCart.ShoppingCarts)
            {
                item.SubTotal = item.Count * item.Product.Price;
                shoppingCart.GrandTotal += item.SubTotal;
            }

            return View(shoppingCart);
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        // POST: ShoppingCartController/Delete/5
        [HttpDelete]
        [Authorize]
        public ActionResult Delete(int id)
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
    }
}
