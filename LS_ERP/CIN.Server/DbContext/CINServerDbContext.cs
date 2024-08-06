using CIN.Domain;
using Microsoft.EntityFrameworkCore;

namespace CIN.Server
{
    public class CINServerDbContext : DbContext
    {
        public CINServerDbContext()
        {

        }

        public CINServerDbContext(DbContextOptions<CINServerDbContext> options)
            : base(options)
        {

        }

        public DbSet<CINServerMetaData> MetaDataList { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
