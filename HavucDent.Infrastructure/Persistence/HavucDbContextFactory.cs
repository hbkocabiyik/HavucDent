using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HavucDent.Infrastructure.Persistence
{
    public class HavucDbContextFactory : IDesignTimeDbContextFactory<HavucDbContext>
    {
        public HavucDbContext CreateDbContext(string[] args)
        {
            // appsettings.json dosyasını okuma
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<HavucDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new HavucDbContext(builder.Options);
        }
    }
}
