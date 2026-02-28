using biztrack.ostad.Domain;

namespace biztrack.ostad.Infrastructure.Interfaces;

public interface IStockRepository : IRepository<Stock, int>
{
    Task<Stock?> GetByProductAndWarehouseAsync(int productId, int warehouseId, CancellationToken cancellationToken = default);
}
