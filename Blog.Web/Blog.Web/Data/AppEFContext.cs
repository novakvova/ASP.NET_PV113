using Blog.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    public class AppEFContext : DbContext
    {
        public AppEFContext(DbContextOptions<AppEFContext> options) : base(options)
        { }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
    }
}
