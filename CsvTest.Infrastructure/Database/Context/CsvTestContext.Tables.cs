using CsvTest.Infrastructure.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace CsvTest.Infrastructure.Database
{
    public partial class CsvTestContext
    {
        public DbSet<CsvData> CsvDatas { get; set; }
        public DbSet<CsvDataWithJson> CsvDatasJson { get; set; }
    }
}
