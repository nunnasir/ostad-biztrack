using biztrack.ostad.Domain;
using biztrack.ostad.Infrastructure.Data;
using biztrack.ostad.Infrastructure.Interfaces;

namespace biztrack.ostad.Infrastructure.Repositories;

public class WarehouseRepository : Repository<Warehouse, int>, IWarehouseRepository
{
    public WarehouseRepository(BizTrackDbContext context) : base(context)
    {
    }
}
