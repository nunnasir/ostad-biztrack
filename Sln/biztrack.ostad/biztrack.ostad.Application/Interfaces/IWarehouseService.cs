using biztrack.ostad.Application.Common;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;

namespace biztrack.ostad.Application.Interfaces;

public interface IWarehouseService
{
    Task<Result<IReadOnlyList<WarehouseListResponse>>> GetAllAsync(bool activeOnly = false, CancellationToken cancellationToken = default);
    Task<Result<WarehouseListResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<int>> CreateAsync(CreateWarehouseRequest request, string? userId = null, CancellationToken cancellationToken = default);
    Task<Result<bool>> UpdateAsync(UpdateWarehouseRequest request, string? userId = null, CancellationToken cancellationToken = default);
}
