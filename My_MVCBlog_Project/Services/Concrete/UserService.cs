using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Services.Abstract;
using System.Net.Http;

namespace My_MVCBlog_Project.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
           
        }


        public ServiceResponse<User> Find(int? id)
        {
            ServiceResponse<User> result = new ServiceResponse<User>
            {
                Data = _context.Users.FirstOrDefault(x => x.Id == id)

            };
            if (result.Data != null)
            {
                result.AddError("Kayıt Bulunamadı");
            }
            return result;
        }


        public ServiceResponse<User> Create(UserViewModel model, HttpContext httpContext)
        {
            ServiceResponse<User> result = new ServiceResponse<User>();
            model.Username = model.Username.Trim().ToLower();
            model.Email = model.Email.Trim().ToLower();
            User user = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsActive = model.IsActive,
                IsAdmin = model.IsAdmin,
                CreatedDate = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.Username),
            };
            if (_context.SaveChanges()==0)
            {
                result.AddError("Bir sorun oluştu,kayıt yapılamadı");
            }
            else
            {
                result.Data = user;
            }
            return result;  
        }

        public ServiceResponse<User> Edit(int id, UserEditViewModel model, HttpContext httpContext)
        {
            ServiceResponse<User> result = new ServiceResponse<User>();
            model.Username = model.Username.Trim().ToLower();
            model.Email = model.Email.Trim().ToLower();
            if (_context.Users.Any(x=>x.Email.ToLower() == model.Email.ToLower() && x.Id != id))
            {
                result.AddError($"'{model.Email}' Email zaten kayıtlıdır");
                return result;
            }
            if (_context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
            {
                result.AddError($"'{model.Username}' Username zaten kayıtlıdır");
                return result;
            }

            var dbUser = _context.Users.Find(id);
            dbUser.Username = model.Username;
            dbUser.Email = model.Email;
            dbUser.CreatedUser = httpContext.Session.GetString(Constants.Username);
            dbUser.IsActive = true;
            dbUser.IsAdmin = false;
            dbUser.CreatedDate = dbUser.CreatedDate;
            dbUser.ModifiedDate = DateTime.Now;
            dbUser.ModifiedUser = httpContext.Session.GetString(Constants.Username);

            if (_context.SaveChanges() == 0)
            {
                result.AddError("Kayıt Yapılamadı");
            }
            return result;
        }

        public ServiceResponse<List<User>> List()
        {
            var users = _context.Users.ToList();
            foreach (var item in users)
            {
                item.Password = string.Empty;
            }
            ServiceResponse<List<User>> result = new ServiceResponse<List<User>>();
            result.Data = users;
            return result;
        }

        public ServiceResponse<User> Login(LoginViewModel model)
        {
            ServiceResponse<User> result = new ServiceResponse<User>();
            model.Username = model.Username.Trim().ToLower();
            User user = _context.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower());
            if (user.Username.ToLower() != model.Username.ToLower() || user.Password.ToLower() != model.Password.ToLower())
            {
                result.AddError("Hatalı kullanıcı adı veya şifre");
            }
            else
            {
                result.Data = user;
            }
            return result;
        }

        public ServiceResponse<object> Register(RegisterViewModel model)
        {
            ServiceResponse<object> result = new ServiceResponse<object>();
            model.Username = model.Username.Trim().ToLower();
            model.Email = model.Email.Trim().ToLower();
            if (_context.Users.Any(x=>x.Email.ToLower() == model.Email.ToLower()))
            {
                result.AddError($"'{model.Email}' mail adresine sahip kullanıcı bulunmaktadır");
                result.AddError($"'{model.Email}' denemelik adresine sahip kullanıcı bulunmaktadır");
                return result;
            }
            if (_context.Users.Any(x=>x.Username.ToLower()==model.Username.ToLower()))
            {
                result.AddError($"'{model.Username}' kullanıcı  adına sahip ,kullanıcı bulunmaktadır");
                return result;
            }
            _context.Users.Add(new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsActive = true,
                IsAdmin = false,
                CreatedUser = "System",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedUser = "",
            });
            if (_context.SaveChanges()==0)
            {
                result.AddError("Kayıt yapılamadı");
            }
            return result;
        }
    }
}
