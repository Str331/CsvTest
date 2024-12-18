using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CsvTest.Infrastructure.Database
{
    public partial class CsvTestContext : DbContext
    {
        private readonly IConfiguration _config;

        public CsvTestContext(IConfiguration config)
        {
            _config = config;
        }

        public CsvTestContext(DbContextOptions<CsvTestContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("CsvTest"));
        }
    }
}
