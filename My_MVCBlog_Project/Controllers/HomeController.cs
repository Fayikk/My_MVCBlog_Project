﻿using Microsoft.AspNetCore.Mvc;
using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Core.Filters;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;
using My_MVCBlog_Project.Services;
using My_MVCBlog_Project.Services.Abstract;
using Org.BouncyCastle.Asn1.Pkcs;
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
        private readonly IUserService _userService;
        private readonly IMailHelper _mailHelper;
        public HomeController(ILogger<HomeController> logger,IMailHelper mailHelper,INoteService noteService, IEBulletinService eBulletinService, ICommentService commentService, ApplicationDbContext context, IUserService userService)
        {
            _noteService = noteService;
            _logger = logger;
            _commentService = commentService;
            _context = context;
            _eBulletinService = eBulletinService;
            _userService = userService;
            _mailHelper = mailHelper;
        }


        public IActionResult ProfileEdit()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ProfileImageSave(IFormFile profileImage)
        {
            try
            {
                string uploadPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
                string fileName = $"userImage_{userId}.jpg";
                string filePath = Path.Combine(uploadPath, fileName);
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    profileImage.CopyTo(stream);
                }
                ViewData["Success"] = "Profil Resminiz Güncellendi";
            }
            catch (Exception)
            {

                ModelState.AddModelError(string.Empty, "Hatalı Resim Ekleme İşlemi Gerçekleştirdiniz");
            }

            return View(nameof(ProfileEdit));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPasswordPost(string email)
        {
            _mailHelper.SendChangePasswordEmail("Parola Değişikliği", email);
            return View(nameof(WarningPage));
        }

        public IActionResult ChangePassword(Guid changePasswordId)
        {
            var user = _userService.FindGuid(changePasswordId);
            ViewData["changePasswordGuid"] = changePasswordId;
            ChangePasswordModel model = new ChangePasswordModel();
            model.Guid = changePasswordId;
            return View(model);
        }

        [HttpPost]

        public IActionResult ImplementChangePassword(ChangePasswordModel model)
        {
            var user = _userService.FindGuid(model.Guid);
            var isSuccess = _userService.ChangePasswordImplement(model);
            return RedirectToAction("Login", "User");

        }

        public IActionResult WarningPage()
        {
            return View();
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