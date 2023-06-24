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
        ServiceResponse<User> Edit(int id,UserEditViewModel model,HttpContext httpContext);

        ServiceResponse<User> Find(int? id);
        ServiceResponse<User> FindEmail(string username);
        ServiceResponse<User> FindGuid(Guid id);

        public ServiceResponse<User> ChangePasswordImplement(ChangePasswordModel model);
    }
}
