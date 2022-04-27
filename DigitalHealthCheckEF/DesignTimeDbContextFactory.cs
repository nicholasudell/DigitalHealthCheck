using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DigitalHealthCheckEF
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Database>
    {
        public Database CreateDbContext(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(currentDirectory + "/../DigitalHealthCheckWeb/appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<Database>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            builder.UseSqlServer(connectionString);

            return new Database(builder.Options);
        }
    }
}