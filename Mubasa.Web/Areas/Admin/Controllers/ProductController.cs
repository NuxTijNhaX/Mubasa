using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using Mubasa.Models.ViewModels;
using Mubasa.Utility;
using Mubasa.Web.Areas.Customer.Controllers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mubasa.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStringLocalizer<HomeController> _localizer;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment, IStringLocalizer<HomeController> homeLocalizer)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _localizer = homeLocalizer;
        }

        // GET: ProductController
        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Product.GetAll(includeProp: "Category,CoverType,Author,Publisher,Supplier");

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
        public IActionResult Create(ProductVM productVM, IFormFile file)
        {
            try
            {
                //if (file != null)
                //{
                //    string[] acceptingImgExtensions = new string[3] { ".jpg", ".jpeg", ".png" };
                //    string fileExtension = Path.GetExtension(file.FileName);
                

                //if (!Array.Exists(acceptingImgExtensions, item => item == fileExtension))
                //{
                //    ModelState.AddModelError("Product.ImgUrl", "Tải đúng định dạng ảnh cho phép");
                //}
                //}

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

                        productVM.Product.ImgUrl = @"\images\product\" + completeFileName;
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

            ProductVM productVM = new ProductVM()
            {
                Product = product,
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

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
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

                        if(productVM.Product.ImgUrl != null)
                        {
                            var oldImgPath = Path.Combine(rootPath, productVM.Product.ImgUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                        }

                        productVM.Product.ImgUrl = @"\images\product\" + completeFileName;
                    }

                    _db.Product.Update(productVM.Product);
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

        // POST: ProductController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _db.Product.GetFirstOrDefault(i => i.Id == id);

                if (product == null)
                {
                    return Json(new { success = false, message = $"{_localizer["Not Found"]}" });
                }

                var imgPath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImgUrl.TrimStart('\\'));
                if(System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }

                _db.Product.Remove(product);
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
