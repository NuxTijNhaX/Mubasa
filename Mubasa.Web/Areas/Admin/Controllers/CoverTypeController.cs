using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Web.Areas.Customer.Controllers;
using Mubasa.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        public CoverTypeController(IUnitOfWork db, IStringLocalizer<HomeController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        // GET: CoverTypeController
        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _db.CoverType.GetAll();

            return View(coverTypes);
        }

        // GET: CoverTypeController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoverTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            try
            {
                if (coverType.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.CoverType.Add(coverType);
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

        // GET: CoverTypeController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = _db.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            try
            {
                if (coverType.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.CoverType.Update(coverType);
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

        // POST: CoverTypeController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var author = _db.CoverType.GetFirstOrDefault(i => i.Id == id);

                if (author == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.CoverType.Remove(author);
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
