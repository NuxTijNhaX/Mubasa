using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using System.Collections.Generic;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;
        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }

        // GET: ProductController
        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Product.GetAll();

            return View(products);
        }

        // GET: ProductController/Create
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _db.Category.GetAll().Select(
                    i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Name,
                    }),
                CoverTypeList = _db.CoverType.GetAll().Select(
                    i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Name,
                    })
            };
            
            return View(productVM);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            try
            {
                //if (!product.Name.All(char.IsLetterOrDigit))
                //{
                //    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
                //}

                if (ModelState.IsValid)
                {
                    _db.Product.Add(product);
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

        // GET: ProductController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Product.GetFirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            try
            {
                if (!product.Name.All(char.IsLetterOrDigit))
                {
                    ModelState.AddModelError("Name", "Vui lòng không sử dụng ký tự đặc biệt.");
                }

                if (ModelState.IsValid)
                {
                    _db.Product.Update(product);
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

        // GET: ProductController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Product.GetFirstOrDefault(i => i.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var product = _db.Product.GetFirstOrDefault(i => i.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                _db.Product.Remove(product);
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
