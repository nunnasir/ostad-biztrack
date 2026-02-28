using Microsoft.EntityFrameworkCore;
using biztrack.ostad.Infrastructure.Data;
using biztrack.ostad.Infrastructure.Interfaces;

namespace biztrack.ostad.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly BizTrackDbContext _context;

    public UnitOfWork(BizTrackDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void RollBack()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.Reload();
                    break;
            }
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
