using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Domain;

namespace biztrack.ostad.Application.Mapping;

public static class ProductMapping
{
    public static Product ToEntity(this CreateProductRequest request) => new()
    {
        Name = request.Name,
        Code = request.Code,
        Description = request.Description,
        Unit = request.Unit,
        LowStockThreshold = request.LowStockThreshold,
        IsActive = request.IsActive,
        CreatedAt = DateTime.UtcNow
    };

    public static void ApplyFrom(this Product entity, UpdateProductRequest request)
    {
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.Description = request.Description;
        entity.Unit = request.Unit;
        entity.LowStockThreshold = request.LowStockThreshold;
        entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public static UpdateProductRequest ToUpdateRequest(this Product entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Code = entity.Code,
        Description = entity.Description,
        Unit = entity.Unit,
        LowStockThreshold = entity.LowStockThreshold,
        IsActive = entity.IsActive
    };

    public static ProductListResponse ToListResponse(this Product product, decimal totalStock)
    {
        var threshold = product.LowStockThreshold ?? 0;
        return new ProductListResponse
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Unit = product.Unit,
            LowStockThreshold = product.LowStockThreshold,
            IsActive = product.IsActive,
            TotalStock = totalStock,
            IsLowStock = threshold > 0 && totalStock <= threshold
        };
    }

    public static ProductDetailResponse ToDetailResponse(this Product product, decimal totalStock, List<StockByWarehouseResponse> byWarehouse) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Code = product.Code,
        Description = product.Description,
        Unit = product.Unit,
        LowStockThreshold = product.LowStockThreshold,
        IsActive = product.IsActive,
        TotalStock = totalStock,
        IsLowStock = (product.LowStockThreshold ?? 0) > 0 && totalStock <= (product.LowStockThreshold ?? 0),
        StockByWarehouse = byWarehouse
    };
}
