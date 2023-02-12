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
    [Authorize(Roles = SD.Role_Admin)]
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IStringLocalizer<HomeController> _localizer;
        public SupplierController(IUnitOfWork db, IStringLocalizer<HomeController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        // GET: SupplierController
        public IActionResult Index()
        {
            IEnumerable<Supplier> suppliers = _db.Supplier.GetAll();

            return View(suppliers);
        }

        // GET: SupplierController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupplierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            try
            {
                if (supplier.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.Supplier.Add(supplier);
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

        // GET: SupplierController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = _db.Supplier.GetFirstOrDefault(c => c.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: SupplierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier supplier)
        {
            try
            {
                if (supplier.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
                {
                    ModelState.AddModelError("Name", $"{_localizer["Special Charactors"]}");
                }

                if (ModelState.IsValid)
                {
                    _db.Supplier.Update(supplier);
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

        // POST: SupplierController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var supplier = _db.Supplier.GetFirstOrDefault(i => i.Id == id);

                if (supplier == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                _db.Supplier.Remove(supplier);
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
