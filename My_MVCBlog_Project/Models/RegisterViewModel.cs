using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string? Password { get; set; }
        [Required]

        public string RePassword { get; set; }  
    }
}
