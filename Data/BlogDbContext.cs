using Microsoft.EntityFrameworkCore;
using Project.Models.Domain;

namespace Project.Data
{
    public class BlogDbContext : DbContext
    {
        // program.cs
        public BlogDbContext(DbContextOptions options) : base(options)
        {

        }

        // models
        // use these to create db tables
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
