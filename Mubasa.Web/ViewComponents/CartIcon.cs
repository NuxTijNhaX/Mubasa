using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Utility;
using System.Security.Claims;

namespace Mubasa.Web.ViewComponents
{
    public class CartIcon : ViewComponent
    {
        private readonly IUnitOfWork _db;
        public CartIcon(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(
                        SD.SessionCart,
                        _db.ShoppingItem
                            .GetAll(i => i.ApplicationUserId == claim.Value)
                        .ToList().Count);
                }

                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
