using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PublisherController : Controller
    {
        private readonly IUnitOfWork _db;
        public PublisherController(IUnitOfWork db)
        {
            _db = db;
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
                if (!publisher.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
                }

                if (ModelState.IsValid)
                {
                    _db.Publisher.Add(publisher);
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
                if (!publisher.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
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

        // GET: PublisherController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _db.Publisher.GetFirstOrDefault(i => i.Id == id);

            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: PublisherController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var publisher = _db.Publisher.GetFirstOrDefault(i => i.Id == id);

                if (publisher == null)
                {
                    return NotFound();
                }

                _db.Publisher.Remove(publisher);
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
