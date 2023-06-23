using Microsoft.AspNetCore.Mvc;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Services;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResponse<User> result = _userService.Login(model);
                if (!result.IsError)
                {
                    HttpContext.Session.SetInt32(Constants.UserId, result.Data.Id);
                    HttpContext.Session.SetString(Constants.Username, result.Data.Username);
                    HttpContext.Session.SetString(Constants.UserEmail, result.Data.Email);
                    HttpContext.Session.SetString(Constants.UserRole, result.Data.IsAdmin == true ? "admin":"member");
                    return RedirectToAction("Index","Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Register(model);
                if (!result.IsError)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item);
                    }
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
