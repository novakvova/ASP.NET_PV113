using Microsoft.EntityFrameworkCore;
using WebBomba.Data.Entities;

namespace WebBomba.Data
{
    public class DataEFContext : DbContext
    {
        public DataEFContext(DbContextOptions<DataEFContext> options)
            : base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
