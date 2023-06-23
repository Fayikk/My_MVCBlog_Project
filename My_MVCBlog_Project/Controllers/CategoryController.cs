using Microsoft.AspNetCore.Mvc;
using My_MVCBlog_Project.Core.Filters;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Services;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Controllers
{
    [AdminFilter]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            ServiceResponse<List<Category>> result = _categoryService.List();
            return View(result.Data);    
        }

        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                ServiceResponse<Category> result = _categoryService.CreateCategory(category,HttpContext);
                if (!result.IsError)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            ServiceResponse<Category> _category = _categoryService.Find(id);
            if (_category.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }
            CategoryEditViewModel model = new()
            {
                Name = _category.Data.Name,
                Description = _category.Data.Description,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id,CategoryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResponse<Category> result = _categoryService.UpdateCategory(id,model,HttpContext);
                if (!result.IsError)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            ServiceResponse<Category> result = _categoryService.Find(id);
            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        public IActionResult DeleteConfirm(int id)
        {
            var result = _categoryService.Remove(id);
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }

    }
}
