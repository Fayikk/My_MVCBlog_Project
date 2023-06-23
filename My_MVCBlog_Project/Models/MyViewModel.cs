using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models.DTO;

namespace My_MVCBlog_Project.Models
{
    public class MyViewModel
    {
        public AddCommentDTO CommentDTO { get; set; }   
        public Note Note { get; set; }
    }
}
