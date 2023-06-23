using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _databaseContext;
        public CategoryService(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ServiceResponse<Category> CreateCategory(CategoryViewModel model, HttpContext httpContext)
        {
            ServiceResponse<Category> result = new ServiceResponse<Category>();
            model.Name = model.Name.Trim().ToLower();
            if (_databaseContext.Categories.Any(x=>x.Name.ToLower() == model.Name.ToLower()))
            {
                result.AddError($"'{model.Name}' kullanıcısı zaten kayıtlıdır");
                return result;
            }
            Category category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                CreatedDate = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.Username),
                ModifiedDate = DateTime.Now,
                ModifiedUser = httpContext.Session.GetString(Constants.Username),
            };
            _databaseContext.Categories.Add(category);
            if (_databaseContext.SaveChanges()==0)
            {
                result.AddError("Bir sorun oluştu,kayıt gerçekleşemedi");
            }
            else
            {
                result.Data = category;
            }
            return result;
        }

        public ServiceResponse<Category> Find(int id)
        {
            ServiceResponse<Category> category = new ServiceResponse<Category>();
            var result = _databaseContext.Categories.Find(id);
            if (result != null)
            {
                category.Data = result;
            }
            return category;    
        }

        public ServiceResponse<List<Category>> List()
        {
            var categories = _databaseContext.Categories.ToList();
            ServiceResponse<List<Category>> result = new ServiceResponse<List<Category>>();
            result.Data = categories;
            return result;
        }

        public ServiceResponse<object> Remove(int id)
        {
            ServiceResponse<object> result = new ServiceResponse<object>();
            var category = _databaseContext.Categories.Find(id);
            if (category != null)
            {
                _databaseContext.Categories.Remove(category);
                if (_databaseContext.SaveChanges()==0)
                {
                    result.AddError("İşleminiz gerçekleştirilemedi");
                }
                else
                {
                    result.Data = category;
                }
            }
            return result;
        }

        public ServiceResponse<Category> UpdateCategory(int id, CategoryEditViewModel model, HttpContext httpContext)
        {
            ServiceResponse<Category> result =new ServiceResponse<Category>();
            model.Name = model.Name.Trim().ToLower();
            if (_databaseContext.Categories.Any(x=>x.Name.ToLower() == model.Name.ToLower() && x.Id != id ))
            {
                result.AddError($"'{model.Name}' kategorisi zaten kayıtlıdır");
                return result;
            }
            var dbCategory = _databaseContext.Categories.Find(id);
            dbCategory.Description = model.Description;
            dbCategory.Name = model.Name;
            dbCategory.CreatedUser = httpContext.Session.GetString(Constants.Username);
            dbCategory.CreatedDate = DateTime.Now;
            dbCategory.ModifiedUser = httpContext.Session.GetString(Constants.Username);
            dbCategory.ModifiedDate = DateTime.Now;

            if (_databaseContext.SaveChanges()==0)
            {
                result.AddError("Kayıt yapılamadı");
            }
            return result;
        }
    }
}
