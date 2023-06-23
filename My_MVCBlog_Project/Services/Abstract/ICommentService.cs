using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;

namespace My_MVCBlog_Project.Services.Abstract
{
    public interface ICommentService
    {
        ServiceResponse<Comment> RemoveComment(int commentId);
        ServiceResponse<Comment> UpdateComment(int commentId);
        ServiceResponse<Comment> Find(int? commentId);
        ServiceResponse<Comment> EditComment(int id, CommentEditViewModel model, HttpContext httpContext);
        ServiceResponse<Comment> AddComment(AddCommentDTO commentDTO);
    }
}
