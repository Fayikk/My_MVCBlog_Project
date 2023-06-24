using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set;}
        public Guid Guid { get; set; }  
    }
}
