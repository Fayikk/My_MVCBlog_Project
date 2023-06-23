using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;

namespace My_MVCBlog_Project.Services.Abstract
{
    public interface IUserService
    {
        public ServiceResponse<object> Register(RegisterViewModel model);
        ServiceResponse<User> Login(LoginViewModel model);
        ServiceResponse<User> Create(UserViewModel model,HttpContext httpContext);
        ServiceResponse<List<User>> List();
    }
}
