using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Entities
{
    public class User : EntityLogBase
    {
        [Key]
        public int Id { get; set; } 
        public string? FullName { get; set; }
        public string? Username { get; set; }  
        public string Email { get; set; }   
        public string? Password { get; set; }    
        public  bool IsActive { get; set; }   
        public bool IsAdmin { get; set; }   
        public virtual List<Note> Notes { get; set; }   
       
    }
}
