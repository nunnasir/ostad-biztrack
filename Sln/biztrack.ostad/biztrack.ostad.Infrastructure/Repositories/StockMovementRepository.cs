using biztrack.ostad.Domain;
using biztrack.ostad.Infrastructure.Data;
using biztrack.ostad.Infrastructure.Interfaces;

namespace biztrack.ostad.Infrastructure.Repositories;

public class StockMovementRepository : Repository<StockMovement, int>, IStockMovementRepository
{
    public StockMovementRepository(BizTrackDbContext context) : base(context)
    {
    }
}
