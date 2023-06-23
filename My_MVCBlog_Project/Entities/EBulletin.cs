using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Entities
{
    public class EBulletin
    {
        [Key]
        public int Id { get; set; }
        [Required,StringLength(50),Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }
        [Display(Name = "Yasaklı")]
        public bool Banned { get; set; }
        [Display(Name = "Oluşturma Tarihi")]

        public DateTime CreatedDate { get; set; }   
    }
}
