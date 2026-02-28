using biztrack.ostad.Application.Common;
using biztrack.ostad.Application.Mapping;
using biztrack.ostad.Application.Interfaces;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Domain;
using biztrack.ostad.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace biztrack.ostad.Application.Implementations;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork)
    {
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<WarehouseListResponse>>> GetAllAsync(bool activeOnly = false, CancellationToken cancellationToken = default)
    {
        Expression<Func<Warehouse, bool>>? predicate = activeOnly ? w => w.IsActive : null;
        var list = await _warehouseRepository.GetAsync(
            predicate: predicate,
            orderBy: q => q.OrderBy(w => w.Name),
            cancellationToken: cancellationToken);
        return Result<IReadOnlyList<WarehouseListResponse>>.SuccessResult(list.Select(w => w.ToListResponse()).ToList());
    }

    public async Task<Result<WarehouseListResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(id, cancellationToken);
        if (warehouse == null)
            return Result<WarehouseListResponse?>.SuccessResult(null);
        return Result<WarehouseListResponse?>.SuccessResult(warehouse.ToListResponse());
    }

    public async Task<Result<int>> CreateAsync(CreateWarehouseRequest request, string? userId = null, CancellationToken cancellationToken = default)
    {
        var warehouse = request.ToEntity();
        warehouse.CreatedBy = userId;
        await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.SuccessResult(warehouse.Id);
    }

    public async Task<Result<bool>> UpdateAsync(UpdateWarehouseRequest request, string? userId = null, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (warehouse == null)
            return Result<bool>.FailResult("Warehouse not found.");
        warehouse.ApplyFrom(request);
        warehouse.UpdatedBy = userId;
        _warehouseRepository.Update(warehouse);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}
