using Microsoft.EntityFrameworkCore;
using biztrack.ostad.Domain;
using biztrack.ostad.Infrastructure.Data;
using biztrack.ostad.Infrastructure.Interfaces;

namespace biztrack.ostad.Infrastructure.Repositories;

public class StockRepository : Repository<Stock, int>, IStockRepository
{
    public StockRepository(BizTrackDbContext context) : base(context)
    {
    }

    public async Task<Stock?> GetByProductAndWarehouseAsync(int productId, int warehouseId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId, cancellationToken);
    }
}
