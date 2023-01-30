using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Utility;
using Mubasa.Web.Areas.Customer.Controllers;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;

        public CategoryController(IUnitOfWork db, IStringLocalizer<HomeController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.Category.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
            {
                ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
            }

            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.Save();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _db.Category.GetFirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
            {
                ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
            }

            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.Save();
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var author = _db.Category.GetFirstOrDefault(i => i.Id == id);

                if (author == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.Category.Remove(author);
                _db.Save();

                return Json(new { success = true, message = $"{_localizer["Delete Successful"]}" });
            }
            catch
            {
                return Json(new { success = false, message = $"{_localizer["Error Deleting Data"]}" });
            }
        }
    }
}
