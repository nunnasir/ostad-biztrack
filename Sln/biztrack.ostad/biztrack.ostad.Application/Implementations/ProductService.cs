using biztrack.ostad.Application.Common;
using biztrack.ostad.Application.Mapping;
using biztrack.ostad.Application.Interfaces;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Domain;
using biztrack.ostad.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace biztrack.ostad.Application.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IStockMovementRepository _movementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IStockMovementRepository movementRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _movementRepository = movementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<ProductListResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAsync(
            predicate: null,
            orderBy: q => q.OrderBy(p => p.Name),
            include: "Stocks",
            tracking: false,
            cancellationToken: cancellationToken);
        var list = products.Select(p =>
        {
            var total = p.Stocks?.Sum(s => s.Quantity) ?? 0;
            return p.ToListResponse(total);
        }).ToList();
        return Result<IReadOnlyList<ProductListResponse>>.SuccessResult(list);
    }

    public async Task<Result<ProductDetailResponse?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product == null)
            return Result<ProductDetailResponse?>.SuccessResult(null);

        var stocks = await _stockRepository.GetAsync(
            predicate: s => s.ProductId == id,
            include: "Warehouse",
            tracking: false,
            cancellationToken: cancellationToken);
        var totalStock = stocks.Sum(s => s.Quantity);
        var byWarehouse = stocks.Select(s => new StockByWarehouseResponse
        {
            WarehouseId = s.WarehouseId,
            WarehouseName = s.Warehouse?.Name ?? "",
            Quantity = s.Quantity
        }).ToList();
        var response = product.ToDetailResponse(totalStock, byWarehouse);
        return Result<ProductDetailResponse?>.SuccessResult(response);
    }

    public async Task<Result<int>> CreateAsync(CreateProductRequest request, string? userId = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var existing = await _productRepository.GetAsync(
                predicate: p => p.Code == request.Code,
                cancellationToken: cancellationToken);
            if (existing.Count > 0)
                return Result<int>.FailResult("A product with this code already exists.");
        }

        var product = request.ToEntity();
        product.CreatedBy = userId;
        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.SuccessResult(product.Id);
    }

    public async Task<Result<bool>> UpdateAsync(UpdateProductRequest request, string? userId = null, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            return Result<bool>.FailResult("Product not found.");

        if (!string.IsNullOrWhiteSpace(request.Code) && request.Code != product.Code)
        {
            var existing = await _productRepository.GetAsync(
                predicate: p => p.Code == request.Code,
                cancellationToken: cancellationToken);
            if (existing.Count > 0)
                return Result<bool>.FailResult("A product with this code already exists.");
        }

        product.ApplyFrom(request);
        product.UpdatedBy = userId;
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product == null)
            return Result<bool>.FailResult("Product not found.");

        _productRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }

    public async Task<Result<IReadOnlyList<LowStockAlertResponse>>> GetLowStockAlertsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAsync(
            predicate: p => p.IsActive && p.LowStockThreshold != null && p.LowStockThreshold > 0,
            include: "Stocks",
            tracking: false,
            cancellationToken: cancellationToken);
        var list = new List<LowStockAlertResponse>();
        foreach (var p in products)
        {
            var total = p.Stocks?.Sum(s => s.Quantity) ?? 0;
            if (total <= (p.LowStockThreshold ?? 0))
                list.Add(new LowStockAlertResponse
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductCode = p.Code,
                    CurrentStock = total,
                    LowStockThreshold = p.LowStockThreshold
                });
        }
        return Result<IReadOnlyList<LowStockAlertResponse>>.SuccessResult(list.OrderBy(x => x.CurrentStock).ToList());
    }

    public async Task<Result<IReadOnlyList<StockMovementListResponse>>> GetMovementHistoryAsync(int? productId, int? warehouseId, int count = 50, CancellationToken cancellationToken = default)
    {
        var movements = await _movementRepository.GetAsync(
            predicate: m =>
                (productId == null || m.ProductId == productId) &&
                (warehouseId == null || m.WarehouseId == warehouseId),
            orderBy: q => q.OrderByDescending(m => m.MovementDate).ThenByDescending(m => m.Id),
            include: "Product,Warehouse",
            tracking: false,
            cancellationToken: cancellationToken);
        var list = movements.Take(count).Select(m => new StockMovementListResponse
        {
            Id = m.Id,
            ProductName = m.Product?.Name ?? "",
            WarehouseName = m.Warehouse?.Name ?? "",
            Quantity = m.Quantity,
            MovementType = m.MovementType.ToString(),
            Reference = m.Reference,
            MovementDate = m.MovementDate,
            CreatedAt = m.CreatedAt
        }).ToList();
        return Result<IReadOnlyList<StockMovementListResponse>>.SuccessResult(list);
    }

    public async Task<Result<bool>> RecordStockMovementAsync(RecordStockMovementRequest request, string? userId = null, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            return Result<bool>.FailResult("Product not found.");
        // Warehouse existence can be checked via repository
        var stock = await _stockRepository.GetByProductAndWarehouseAsync(request.ProductId, request.WarehouseId, cancellationToken);
        if (stock == null)
        {
            if (request.Quantity < 0)
                return Result<bool>.FailResult("Cannot record stock out: no stock record exists for this product and warehouse.");
            stock = new Stock
            {
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                Quantity = request.Quantity,
                UpdatedAt = DateTime.UtcNow
            };
            await _stockRepository.AddAsync(stock, cancellationToken);
        }
        else
        {
            stock.Quantity += request.Quantity;
            if (stock.Quantity < 0)
                return Result<bool>.FailResult("Stock quantity cannot become negative.");
            stock.UpdatedAt = DateTime.UtcNow;
            _stockRepository.Update(stock);
        }

        var movement = new StockMovement
        {
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            Quantity = request.Quantity,
            MovementType = (Domain.StockMovementType)request.MovementType,
            Reference = request.Reference,
            MovementDate = DateTime.UtcNow,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };
        await _movementRepository.AddAsync(movement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}
