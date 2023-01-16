using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _db;

        public CoverTypeController(IUnitOfWork db)
        {
            _db = db;
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
                if (!coverType.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
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
                if (!coverType.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
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

        // GET: CoverTypeController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = _db.CoverType.GetFirstOrDefault(i => i.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverTypeController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var coverType = _db.CoverType.GetFirstOrDefault(i => i.Id == id);

                if (coverType == null)
                {
                    return NotFound();
                }

                _db.CoverType.Remove(coverType);
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
