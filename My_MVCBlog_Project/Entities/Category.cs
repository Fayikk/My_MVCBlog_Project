using System.ComponentModel.DataAnnotations;

namespace My_MVCBlog_Project.Entities
{
    
    public class Category : EntityLogBase
    {
        [Key]
        public int Id { get; set; }
        [Required,StringLength(40),Display(Name="Kategori Adı")]  
        public string Name { get; set; }
        [StringLength(160),Display(Name="Açıklama")]
        public string Description { get; set; } 
        public virtual List<Note> Notes { get; set; }
    }
}
