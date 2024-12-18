using CsvTest.Infrastructure.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace CsvTest.Infrastructure.Database
{
    public partial class CsvTestContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CsvData>(entity =>
            {
                entity.HasKey(e => e.CsvDataID);
            });

            modelBuilder.Entity<CsvDataWithJson>(entity =>
            {
                entity.HasKey(e => e.CsvDataWithJsonID);
            });
        }
    }
}
