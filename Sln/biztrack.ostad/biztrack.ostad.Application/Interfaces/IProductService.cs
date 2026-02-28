using biztrack.ostad.Application.Common;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Domain;

namespace biztrack.ostad.Application.Interfaces;

public interface IProductService
{
    Task<Result<IReadOnlyList<ProductListResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<ProductDetailResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<int>> CreateAsync(CreateProductRequest request, string? userId = null, CancellationToken cancellationToken = default);
    Task<Result<bool>> UpdateAsync(UpdateProductRequest request, string? userId = null, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<LowStockAlertResponse>>> GetLowStockAlertsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StockMovementListResponse>>> GetMovementHistoryAsync(int? productId, int? warehouseId, int count = 50, CancellationToken cancellationToken = default);
    /// <summary>Records a stock in/out movement and updates stock balance. Used by purchases, sales, or manual adjustments.</summary>
    Task<Result<bool>> RecordStockMovementAsync(RecordStockMovementRequest request, string? userId = null, CancellationToken cancellationToken = default);
}
