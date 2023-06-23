using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }    
    }
}
