using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LogBook.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LogBookDbContext>
    {
        public LogBookDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.design.json")
                .Build();

            var builder = new DbContextOptionsBuilder<LogBookDbContext>();

            var connectionString = configuration.GetConnectionString("Default");
            builder.UseNpgsql(connectionString, o =>
                o.UseNetTopologySuite());

            return new LogBookDbContext(builder.Options);
        }
    }
}
