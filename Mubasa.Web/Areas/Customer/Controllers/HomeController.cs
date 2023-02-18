using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Utility;
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
            var products = _db.Product.GetAll().Take(12);

            return View(products);
        }

        public IActionResult ListProducts(string? search, int? from_price, int? to_price)
        {
            IEnumerable<Product> products; 
            if (search == null)
            {
                products = _db.Product.GetAll();
            }
            else
            {
                products = _db.Product.GetAll(i => i.Name.Contains(search));
            }

            if (from_price != null)
            {
                products = products.Where(i => from_price < i.Price && i.Price <= to_price);
            }

            return View(products);
        }

        public IActionResult Details(int productId)
        {
            var product = _db.Product.GetFirstOrDefault(x => x.Id == productId, includeProp: "Category,CoverType,Author,Publisher,Supplier");

            var shoppingItem = new ShoppingItem()
            {
                Count = 1,
                ProductId = productId,
                Product = product,
            };

            return View(shoppingItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingItem shoppingItem)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingItem.ApplicationUserId = claim.Value;

            AddProductToCart(shoppingItem);

            return RedirectToAction("Details", new{ productId = shoppingItem.ProductId });
        }

        [Authorize]
        public IActionResult AddToCart(int productId, string returnUrl = null)
        {
            try
            {
                returnUrl ??= Url.Content("~/");

                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShoppingItem shoppingItem = new()
                {
                    ProductId = productId,
                    ApplicationUserId = claim.Value,
                    Count = 1,
                };

                AddProductToCart(shoppingItem);

                return LocalRedirect(returnUrl);
            }
            catch (Exception)
            {
                return LocalRedirect(Url.Content("~/"));
            }
            
        }

        private void AddProductToCart(ShoppingItem shoppingItem)
        {
            try
            {
                var shoppingItemDb = _db.ShoppingItem.GetFirstOrDefault(x => 
                    x.ApplicationUserId == shoppingItem.ApplicationUserId && 
                    x.ProductId == shoppingItem.ProductId);

                if (shoppingItemDb != null)
                {
                    shoppingItemDb.Count += shoppingItem.Count;
                    _db.ShoppingItem.Update(shoppingItemDb);
                    _db.Save();
                }
                else
                {
                    _db.ShoppingItem.Add(shoppingItem);
                    _db.Save();

                    HttpContext.Session.SetInt32(
                        SD.SessionCart,
                        _db.ShoppingItem.GetAll(i => 
                                i.ApplicationUserId == shoppingItem.ApplicationUserId)
                            .ToList()
                            .Count);
                }

                TempData["success"] = "Đã thêm sản phẩm vào giỏ.";
            }
            catch (Exception)
            {
                TempData["failure"] = "Đã có lỗi, vui lòng thử lại";
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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