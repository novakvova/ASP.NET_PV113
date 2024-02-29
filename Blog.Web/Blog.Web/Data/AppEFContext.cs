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
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PostTagEntity> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PostTagEntity>(ur =>
            {
                ur.HasKey(ur => new { ur.PostId, ur.TagId });
            });
        }
    }
}
