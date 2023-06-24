using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Services;
using My_MVCBlog_Project.Services.Abstract;
using My_MVCBlog_Project.Services.Concrete;
using System.Net.Mail;

namespace My_MVCBlog_Project.Controllers
{
    public class EBulletinController : Controller
    {
        private readonly IEBulletinService _eBulletinService;
        private readonly IMailHelper _mailHelper;
        public EBulletinController(IEBulletinService ebulletinService, IMailHelper mailHelper)
        {
            _eBulletinService = ebulletinService;
            _mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            var result = _eBulletinService.ListEBulletin();
            return View(result.Data);
        }

        public IActionResult SendEmails()
        {
            LoadBulletinSelectDataView();
            return View();
        }
        [HttpPost]
        public IActionResult SendEmails(EBulletinSendEmailViewModel model)
        {
            LoadBulletinSelectDataView();
            _mailHelper.SendEmail(model.Subject, model.Text, model.Emails.ToArray());
            return RedirectToAction(nameof(Index));
        }

      

        private void LoadBulletinSelectDataView()
        {
            List<EBulletin> categories = _eBulletinService.ListExceptBanned().Data;
            List<SelectListItem> selectListItem = categories.Select(x => new SelectListItem(x.Email, x.Email.ToString())).ToList();
            ViewData["bulletins"] = selectListItem;
        }
    }
}
