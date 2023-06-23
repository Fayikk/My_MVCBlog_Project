using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Entities
{
    public class Note : EntityLogBase
    {
        [Key]
        public int Id { get; set; }
        [Required,StringLength(80),Display(Name ="Başlık")]
        public string Title { get; set; }
        [Required, StringLength(250), Display(Name = "Özet")]

        public string Summary { get; set; }
        [Display(Name ="Detay")] 
        
        public string Detail { get; set; }
        [Display(Name = "Taslak")]

        public bool IsDraft { get; set; }
        [Display(Name = "Kategori")]

        public int CategoryId { get; set; }
        [Display(Name = "Yazar")]

        public int OwnerId { get; set; }    
        public virtual User Owner { get; set; } 
        public Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; } 

    }
}
