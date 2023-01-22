using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _db;
        public AuthorController(IUnitOfWork db)
        {
            _db = db;
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
                if (!author.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
                }

                if (ModelState.IsValid)
                {
                    _db.Author.Add(author);
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
                if (!author.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
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

        // GET: AuthorController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _db.Author.GetFirstOrDefault(i => i.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var author = _db.Author.GetFirstOrDefault(i => i.Id == id);

                if (author == null)
                {
                    return NotFound();
                }

                _db.Author.Remove(author);
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
