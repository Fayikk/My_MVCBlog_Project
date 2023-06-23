using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Models.DTO
{
    public class AddCommentDTO
    {
        public string Text { get; set; }    
        public int? UserId { get; set; }    
        public int? NoteId { get; set; }    
        public string? CreatedUser { get; set; } 
        public DateTime CreatedDate { get; set;}
        public string? ModifiedUser { get; set; }    
        public DateTime ModifiedDate { get; set;}

    }
}
