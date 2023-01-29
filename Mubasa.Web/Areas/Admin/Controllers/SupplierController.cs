using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _db;
        public SupplierController(IUnitOfWork db)
        {
            _db = db;
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
                if (!supplier.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
                }

                if (ModelState.IsValid)
                {
                    _db.Supplier.Add(supplier);
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
                if (!supplier.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
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

        // GET: SupplierController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = _db.Supplier.GetFirstOrDefault(i => i.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: SupplierController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var supplier = _db.Supplier.GetFirstOrDefault(i => i.Id == id);

                if (supplier == null)
                {
                    return NotFound();
                }

                _db.Supplier.Remove(supplier);
                _db.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
