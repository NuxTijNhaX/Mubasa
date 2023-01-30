using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility;
using System.Collections.Generic;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
                AuthorList = _db.Author.GetAll().Select(
                    i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Name,
                    }),
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
                    }),
                PublisherList = _db.Publisher.GetAll().Select(
                    i => new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Name,
                    }),
                SupplierList = _db.Supplier.GetAll().Select(
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
        public IActionResult Create(ProductVM productVM, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(file != null)
                    {
                        string rootPath = _webHostEnvironment.WebRootPath;
                        string newFileName = file.FileName + Guid.NewGuid().ToString();
                        string storedFolderPath = Path.Combine(rootPath, @"images\product");
                        string completeFileName = newFileName + Path.GetExtension(file.FileName);

                        string completeFilePath = Path.Combine(storedFolderPath, completeFileName);
                        using (var fileStream = new FileStream(completeFilePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productVM.Product.ImgUrl = @"images\product" + completeFileName;
                    }

                    _db.Product.Add(productVM.Product);
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
                if (product.Name.All((ch) => Extensions.IsInvalidCharactor(ch)))
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
