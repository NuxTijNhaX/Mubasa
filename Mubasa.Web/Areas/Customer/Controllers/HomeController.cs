using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Mubasa.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int productId)
        {
            var product = _db.Product.GetFirstOrDefault(x => x.Id == productId, includeProp: "Category,CoverType,Author,Publisher,Supplier");

            var shoppingCart = new ShoppingCart()
            {
                Count = 1,
                ProductId = productId,
                Product = product,
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            var shoppingCartDb = _db.ShoppingCart.GetFirstOrDefault(x => x.ApplicationUserId == shoppingCart.ApplicationUserId && x.ProductId == shoppingCart.ProductId);
            
            if (shoppingCartDb != null)
            {
                shoppingCartDb.Count += shoppingCart.Count;
                _db.ShoppingCart.Update(shoppingCartDb);
            } else
            {
                _db.ShoppingCart.Add(shoppingCart);
            }
            
            _db.Save();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    }
            );

            return LocalRedirect(returnUrl);
        }
    }
}