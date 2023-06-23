using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;

namespace My_MVCBlog_Project.Services.Abstract
{
    public interface ICategoryService
    {
        ServiceResponse<List<Category>> List();
        ServiceResponse<Category> CreateCategory(CategoryViewModel model,HttpContext httpContext);
        ServiceResponse<Category> Find(int id);
        ServiceResponse<Category> UpdateCategory(int id,CategoryEditViewModel model, HttpContext httpContext);
        ServiceResponse<object> Remove(int id);
    }
}
