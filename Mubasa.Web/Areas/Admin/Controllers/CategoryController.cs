using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _db;

        public CategoryController(IUnitOfWork db)
        {
            _db = db;
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
            if (!category.Name.All(char.IsLetterOrDigit))
            {
                ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
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
            if (!category.Name.All(char.IsLetterOrDigit))
            {
                ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
            }

            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.Save();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.Category.GetFirstOrDefault(i => i.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _db.Category.Remove(category);
            _db.Save();

            return RedirectToAction("Index");
        }
    }
}
