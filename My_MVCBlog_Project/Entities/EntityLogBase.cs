namespace My_MVCBlog_Project.Entities
{
    public class EntityLogBase
    {
        
        public string? CreatedUser { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedUser { get; set; }    
        public DateTime? ModifiedDate { get; set; }  
    }
}
