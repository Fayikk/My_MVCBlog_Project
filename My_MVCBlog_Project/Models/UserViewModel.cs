namespace My_MVCBlog_Project.Models
{
    public class UserViewModel
    {
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? RePassword { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
