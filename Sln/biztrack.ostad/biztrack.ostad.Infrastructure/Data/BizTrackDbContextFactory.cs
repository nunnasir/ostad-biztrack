using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace biztrack.ostad.Infrastructure.Data;

/// <summary>
/// Used by EF Core tools at design time (e.g. migrations) when the startup project does not reference Infrastructure.
/// </summary>
public class BizTrackDbContextFactory : IDesignTimeDbContextFactory<BizTrackDbContext>
{
    public BizTrackDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            basePath = Path.Combine(basePath, "..", "biztrack.ostad.Web");
        if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            basePath = Path.Combine(Directory.GetCurrentDirectory(), "biztrack.ostad.Web");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BizTrackDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=DESKTOP-0F0IUO7;Database=BizTrackDb;User Id=ostad;Password=abc123;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);

        return new BizTrackDbContext(optionsBuilder.Options);
    }
}
