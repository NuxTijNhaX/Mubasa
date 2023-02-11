using Microsoft.AspNetCore.Authorization;
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
    [Authorize(SD.Role_Admin)]
    public class PublisherController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        public PublisherController(IUnitOfWork db, IStringLocalizer<HomeController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        // GET: PublisherController
        public IActionResult Index()
        {
            IEnumerable<Publisher> publishers = _db.Publisher.GetAll();

            return View(publishers);
        }

        // GET: PublisherController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublisherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publisher publisher)
        {
            try
            {
                if (publisher.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.Publisher.Add(publisher);
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

        // GET: PublisherController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _db.Publisher.GetFirstOrDefault(c => c.Id == id);

            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: PublisherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Publisher publisher)
        {
            try
            {
                if (publisher.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.Publisher.Update(publisher);
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

        // POST: PublisherController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var publisher = _db.Publisher.GetFirstOrDefault(i => i.Id == id);

                if (publisher == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.Publisher.Remove(publisher);
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
