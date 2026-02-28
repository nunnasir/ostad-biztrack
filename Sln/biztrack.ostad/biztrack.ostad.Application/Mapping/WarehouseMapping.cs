using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Domain;

namespace biztrack.ostad.Application.Mapping;

public static class WarehouseMapping
{
    public static Warehouse ToEntity(this CreateWarehouseRequest request) => new()
    {
        Name = request.Name,
        Code = request.Code,
        Address = request.Address,
        IsActive = request.IsActive,
        CreatedAt = DateTime.UtcNow
    };

    public static void ApplyFrom(this Warehouse entity, UpdateWarehouseRequest request)
    {
        entity.Name = request.Name;
        entity.Code = request.Code;
        entity.Address = request.Address;
        entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public static UpdateWarehouseRequest ToUpdateRequest(this Warehouse entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Code = entity.Code,
        Address = entity.Address,
        IsActive = entity.IsActive
    };

    public static WarehouseListResponse ToListResponse(this Warehouse entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Code = entity.Code,
        Address = entity.Address,
        IsActive = entity.IsActive
    };
}
