using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace My_MVCBlog_Project.Core.Filters
{
    public class AdminFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Session.GetString(Constants.UserRole).ToLower() != "admin")
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
            }
        }
    }
}
