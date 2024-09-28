using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Myshop.DataAccess.Data;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;


namespace Miso.Service.Areas.Admin
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        public CategoryController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfwork.Category.GetAll();  //_context.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Add(category);
                //_context.SaveChanges();
                _unitOfwork.Category.Add(category);
                _unitOfwork.Complete();
                TempData["Create"] = "Create Has been successfuly";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
            {
                NotFound();
            }
            //var CategoryInDb = _context.Categories.Find(id);
            var CategoryInDb = _unitOfwork.Category.GetFirstorDefault(c => c.Id == id);

            return View(CategoryInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Update(category);
                // _context.SaveChanges();
                _unitOfwork.Category.Update(category);
                _unitOfwork.Complete();
                TempData["Update"] = "Update Has been successfuly";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                NotFound();
            }
            var CategoryInDb = _unitOfwork.Category.GetFirstorDefault(c => c.Id == id);
            return View(CategoryInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var CategoryInDb = _unitOfwork.Category.GetFirstorDefault(c => c.Id == id);
            if (CategoryInDb is null)
            {
                NotFound();
            }
            //_context.Categories.Remove(CategoryInDb);
            _unitOfwork.Category.Remove(CategoryInDb);
            //_context.SaveChanges();
            _unitOfwork.Complete();
            TempData["Delete"] = "Delete Has been successfuly";
            return RedirectToAction("Index");
        }
    }
}
