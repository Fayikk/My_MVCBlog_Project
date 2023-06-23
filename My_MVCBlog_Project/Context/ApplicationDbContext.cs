using Microsoft.EntityFrameworkCore;
using My_MVCBlog_Project.Entities;

namespace My_MVCBlog_Project.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EBulletin> EBulletins { get; set; }

    }
}
