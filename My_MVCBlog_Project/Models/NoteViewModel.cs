using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Models
{
    public class NoteViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required, StringLength(250), Display(Name = "Özet")]

        public string Summary { get; set; }
        [Display(Name = "Detay")]

        public string Detail { get; set; }
        [Display(Name = "Taslak")]

        public bool IsDraft { get; set; }
        [Display(Name = "Kategori")]

        public int CategoryId { get; set; }
        [Display(Name = "Yazar")]

        public int OwnerId { get; set; }
    }
}
