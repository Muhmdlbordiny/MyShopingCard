using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Myshop.DataAccess.Data;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using Myshop.Entities.ViewsModels;


namespace Miso.Service.Areas.Admin
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfwork _unitOfwork;
        public ProductsController(IUnitOfwork unitOfwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfwork = unitOfwork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
           // var product = _unitOfwork.Product.GetAll();  //_context.Categories.ToList();
           // return View(product);
           return View();
        }
        public IActionResult GetData()
        {
            var product = _unitOfwork.Product.GetAll(includeword:"Category");
            return Json(new {data=product});
        }
        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                product = new Products(),
                CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })

            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM ProductVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string Rootpath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(Rootpath,@"Images\Products");
                    var Ext = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(upload, filename + Ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    ProductVM.product.Image = @"Images\products\" + filename + Ext;
                }
                //_context.Categories.Add(Products);
                //_context.SaveChanges();
                _unitOfwork.Product.Add(ProductVM.product);
                _unitOfwork.Complete();
                TempData["Create"] = "Create Has been successfuly";

                return RedirectToAction("Index");
            }
            return View(ProductVM.product);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
            {
                NotFound();
            }
            //var ProductsInDb = _context.Categories.Find(id);
            //var ProductsInDb = _unitOfwork.Product.GetFirstorDefault(c => c.Id == id);
            ProductVM productVM = new ProductVM()
            {
                product = _unitOfwork.Product.GetFirstorDefault(c => c.Id == id),
                CategoryList = _unitOfwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })

            };
            return View(productVM);
            //return View(ProductsInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM ProductVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string Rootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(Rootpath, @"Images\Products");
                    var Ext = Path.GetExtension(file.FileName);
                    if (ProductVM.product.Image != null)
                    {
                        var oldimg = Path.Combine(Rootpath, ProductVM.product.Image.TrimStart('\\'));
                        if(System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, filename + Ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    ProductVM.product.Image = @"Images\products\" + filename + Ext;
                }
                //_context.Categories.Update(Products);
                // _context.SaveChanges();
                _unitOfwork.Product.Update(ProductVM.product);
                _unitOfwork.Complete();
                TempData["Update"] = "Update Has been successfuly";

                return RedirectToAction("Index");
            }
            return View(ProductVM.product);
        }
        //public IActionResult Delete(int? id)
        //{
        //    if (id is null || id == 0)
        //    {
        //        NotFound();
        //    }
        //    var ProductsInDb = _unitOfwork.Product.GetFirstorDefault(c => c.Id == id);
        //    return View(ProductsInDb);
        //}
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var ProductsInDb = _unitOfwork.Product.GetFirstorDefault(p => p.Id == id);
            if (ProductsInDb == null)
            {
                //NotFound();
               return Json(new { succees = false, message = "Error while Deleting" });
            }
            //_context.Categories.Remove(ProductsInDb);
            _unitOfwork.Product.Remove(ProductsInDb);
            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, ProductsInDb.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            //_context.SaveChanges();
            _unitOfwork.Complete();
            return Json(new { succees = true, message = "file has been Deleted" });

            /// TempData["Delete"] = "Delete Has been successfuly";
            ///return RedirectToAction("Index");
        }
    }
}
