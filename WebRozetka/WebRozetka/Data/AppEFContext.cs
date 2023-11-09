using Microsoft.EntityFrameworkCore;
using WebRozetka.Data.Entities;

namespace WebRozetka.Data
{
    public class AppEFContext : DbContext
    {
        public AppEFContext(DbContextOptions<AppEFContext> options)
            :base(options)
        {  }
        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
