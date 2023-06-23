using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Entities
{
    public class Comment : EntityLogBase
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Metin Alanın Girilmesi Zorunludur"),StringLength(250),Display(Name = "Yorum")]
        public string Text { get; set; }
        [Display(Name = "Kullanıcı")]
        public int? UserId { get; set; } 
        public virtual User User { get; set; }
        [Display(Name = "Notlar")]
        public int? NoteId { get; set;}
        public Note Note { get; set; }
    }
}
