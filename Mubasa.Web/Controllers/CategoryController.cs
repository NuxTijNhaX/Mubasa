using Microsoft.AspNetCore.Mvc;

namespace Mubasa.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
