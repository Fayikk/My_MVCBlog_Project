using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;
using My_MVCBlog_Project.Services;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Controllers
{
    public class NoteController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService,ICategoryService categoryService)
        {
            _noteService = noteService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            return View(_noteService.GetAllNotes(userId).Data);
        }


        public IActionResult Create()
        {
            LoadCategorySelectDataView();
            return View();
        }

        [HttpPost]
        public IActionResult Create(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _noteService.CreateNote(model, HttpContext);
                if (!result.IsError)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
            }
            LoadCategorySelectDataView();
            return View(model);
        }


        public IActionResult Edit(int id)
        {
            LoadCategorySelectDataView();
            ServiceResponse<Note> result = _noteService.Find(id);
            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            if (result.Data == null || (result.Data != null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }
            NoteEditViewModel model = new()
            {
                Title = result.Data.Title,
                CategoryId = result.Data.CategoryId,
                Summary = result.Data.Summary,
                Detail = result.Data.Detail,
                IsDraft = result.Data.IsDraft,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, NoteEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResponse<Note> result = _noteService.Edit(id, model, HttpContext);
                if (!result.IsError)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);

                }
            }
            LoadCategorySelectDataView();
            return View(model);
        }


        public IActionResult Delete(int id) 
        {
            ServiceResponse<Note> result = _noteService.Find(id);
            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            if (result.Data == null || (result.Data != null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);

        }
   
    
        public IActionResult DeleteNote(int id)
        {
            var result = _noteService.Find(id);
            if (result.Data != null)
            {
                _noteService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        private void LoadCategorySelectDataView()
        {
            List<Category> categories = _categoryService.List().Data;
            List<SelectListItem> selectListItem = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
            ViewData["categories"] = selectListItem;
        }
    }
}
