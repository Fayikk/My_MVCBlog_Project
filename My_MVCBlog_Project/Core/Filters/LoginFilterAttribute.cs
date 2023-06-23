using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace My_MVCBlog_Project.Core.Filters
{
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Session.GetInt32(Constants.UserId).GetValueOrDefault() == 0)
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
        }
    }
}
