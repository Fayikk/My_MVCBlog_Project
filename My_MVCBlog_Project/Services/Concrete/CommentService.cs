
using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Services.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ServiceResponse<Comment> AddComment(AddCommentDTO commentDTO)
        {
            ServiceResponse<Comment> response = new ServiceResponse<Comment>();
            var user = _context.Users.Find(commentDTO.UserId);
            Comment comment = new Comment
            {
                NoteId = commentDTO.NoteId,
                Text = commentDTO.Text,
                UserId = commentDTO.UserId,
                CreatedDate = DateTime.Now,
                CreatedUser = user.Username,
                ModifiedDate = DateTime.Now,
                ModifiedUser = user.ModifiedUser,
            };
            _context.Comments.Add(comment);
            int? comId = comment.NoteId;
            if (_context.SaveChanges() == 0)
            {
                response.AddError("Yorum yaparken hata oluştu");
            }
            response.Data = Find(comId).Data;
            return response;
        }

        public ServiceResponse<Comment> EditComment(int id, CommentEditViewModel model, HttpContext httpContext)
        {
            ServiceResponse<Comment> result = new ServiceResponse<Comment>();
            var editComment = _context.Comments.Find(id);
            if (editComment != null)
            {
                editComment.Text = model.CommentText;
                editComment.ModifiedUser = httpContext.Session.GetString(Constants.Username);
                editComment.ModifiedDate = DateTime.UtcNow;
                editComment.CreatedUser = httpContext.Session.GetString(Constants.Username);
                editComment.CreatedDate = editComment.CreatedDate;
                result.Data= editComment;
            }
            _context.SaveChanges();
            return result;
        }

        public ServiceResponse<Comment> Find(int? commentId)
        {
            ServiceResponse<Comment> result = new ServiceResponse<Comment>();
            var comment = _context.Comments.Find(commentId);
            if (comment == null)
            {
                result.AddError("Aranan yorum bulunamadı");
                return result;
            }
            else
            {
                result.Data = comment;
                return result;
            }
        }

        public ServiceResponse<Comment> RemoveComment(int commentId)
        {
            ServiceResponse<Comment> result = new ServiceResponse<Comment>();
            var comment = _context.Comments.Find(commentId);
            if (comment == null)
            {
                result.AddError("Aranan yorum bulunamadı");
                return result;
            }
            else
            {
                _context.Remove(comment);
                if (_context.SaveChanges()==0)
                {
                    result.AddError("Kaydedilemedi");
                    return result;
                }
                else
                {
                    result.IsError = false;
                    return result;
                }
            }

        }

        public ServiceResponse<Comment> UpdateComment(int commentId)
        {
            ServiceResponse<Comment> result = new ServiceResponse<Comment>();
            var updateComment = _context.Comments.Find(commentId);
            if (updateComment == null)
            {
                result.AddError("Yorum bulunamadı");
                return result;
            }
            else
            {
                _context.Update(updateComment);
                _context.SaveChanges();
                result.Data = Find(commentId).Data;
                return result;
            }
        }
    }
}
