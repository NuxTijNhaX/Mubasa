using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Utility;
using Mubasa.Web.Areas.Customer.Controllers;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        public AuthorController(IUnitOfWork db, IStringLocalizer<HomeController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        // GET: AuthorController
        public IActionResult Index()
        {
            IEnumerable<Author> authors = _db.Author.GetAll();

            return View(authors);
        }

        // GET: AuthorController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            try
            {
                if (author.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.Author.Add(author);
                    _db.Save();

                    TempData["success"] = $"{_localizer["Create Successful"]}";

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _db.Author.GetFirstOrDefault(c => c.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author author)
        {
            try
            {
                if (author.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.Author.Update(author);
                    _db.Save();

                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // POST: AuthorController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var author = _db.Author.GetFirstOrDefault(i => i.Id == id);

                if (author == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.Author.Remove(author);
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
