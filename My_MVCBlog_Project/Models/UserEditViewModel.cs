using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Models
{
    public class UserEditViewModel
    {
        [Required,StringLength(60),Display(Name = "Adı-Soyadı")]
        public string? FullName { get; set; }
        [Required, StringLength(50), Display(Name = "Kullanıcı Adı")]

        public string? Username { get; set; }
        public string Email { get; set; }
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }
        [Display(Name = "Yönetici")]
        public bool IsAdmin { get; set; }
    }
}
