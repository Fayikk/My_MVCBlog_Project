using Microsoft.AspNetCore.Mvc;
using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Core.Filters;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;
using My_MVCBlog_Project.Services;
using My_MVCBlog_Project.Services.Abstract;
using System.Diagnostics;

namespace My_MVCBlog_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoteService _noteService;
        private readonly ICommentService _commentService;
        private readonly ApplicationDbContext _context;
        private readonly IEBulletinService _eBulletinService;

        public HomeController(ILogger<HomeController> logger,INoteService noteService,IEBulletinService eBulletinService, ICommentService commentService,ApplicationDbContext context)
        {
            _noteService = noteService;
            _logger = logger;
            _commentService = commentService;
            _context = context;
            _eBulletinService = eBulletinService;
        }

        public IActionResult Index(int? categoryId,string mode)
        {

            if (categoryId == null && string.IsNullOrEmpty(mode))
            {
                return View(_noteService.List(null, null).Data);
            }
            else
            {
                return View(_noteService.List(categoryId, mode).Data);
            }
        }

        public IActionResult Unauthorized()
        {
            return View();
        }


        [LoginFilter]
        public IActionResult GetNoteComments(int noteId)
        {
            ServiceResponse<Note> result = _noteService.Find(noteId);
            AddCommentDTO comment = new AddCommentDTO();
            comment.NoteId = noteId;
            MyViewModel model = new()
            {
                CommentDTO = comment,
                Note = result.Data,
            };
            if (result.Data == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateComment(AddCommentDTO commentDTO)
        {
            int? userId = HttpContext.Session.GetInt32(Constants.UserId);
            commentDTO.UserId = userId;
            var result = _commentService.AddComment(commentDTO);
            if (result.IsError == true)
            {
                return NotFound();
            }
            return Redirect($"/Home/GetNoteComments?noteId={commentDTO.NoteId}");
        }

        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            var result = _commentService.RemoveComment(id);
            var note = _context.Notes.Find(comment.NoteId);
            if (result.IsError == true)
            {
                return NotFound();
            }
            return Redirect($"/Home/GetNoteComments?noteId={note.Id}");
        }

        public IActionResult SaveEBulletinEmail(string EBulletinEmail)
        {
            ServiceResponse<object> result = new ServiceResponse<object>();
            result = _eBulletinService.Create(EBulletinEmail);
            return RedirectToAction(nameof(Index));
        }

        public  IActionResult EditComment(int id)
        {
            var result = _commentService.Find(id);
            CommentEditViewModel model = new();
            if (result.Data == null)
            {
                return RedirectToAction("Index");
            }
            model.CommentText = result.Data.Text;
            model.CommentId = result.Data.Id;
            return View(model);
            
        }


        [HttpPost]
        public IActionResult EditComment(CommentEditViewModel model)
        {
            var result = _commentService.EditComment(model.CommentId, model, HttpContext);
            var comment = _context.Comments.Find(model.CommentId);
            var note = _context.Notes.Find(comment.NoteId);
            if (result.Data == null)
            {
                return NotFound();
            }
            return Redirect($"/Home/GetNoteComments?noteId={note.Id}");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}