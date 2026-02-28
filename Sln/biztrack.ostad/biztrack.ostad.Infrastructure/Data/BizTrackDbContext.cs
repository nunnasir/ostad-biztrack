using Microsoft.EntityFrameworkCore;
using biztrack.ostad.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace biztrack.ostad.Infrastructure.Data;

public class BizTrackDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public BizTrackDbContext(DbContextOptions<BizTrackDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(BizTrackDbContext).Assembly);
    }
}
