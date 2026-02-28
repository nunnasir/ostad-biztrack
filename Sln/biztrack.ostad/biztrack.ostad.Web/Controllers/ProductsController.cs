using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using biztrack.ostad.Application.Interfaces;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Web.Constants;

namespace biztrack.ostad.Web.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IWarehouseService _warehouseService;

    public ProductsController(IProductService productService, IWarehouseService warehouseService)
    {
        _productService = productService;
        _warehouseService = warehouseService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _productService.GetAllAsync(cancellationToken);
        if (!result.Success)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return View(new List<ProductListResponse>());
        }
        return View(result.Data ?? new List<ProductListResponse>());
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetByIdAsync(id, cancellationToken);
        if (!result.Success)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return RedirectToAction(nameof(Index));
        }
        if (result.Data == null)
        {
            TempData[TempDataKeys.ErrorMessage] = ProductMessages.NotFound;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateProductRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(request);
        var result = await _productService.CreateAsync(request, User.Identity?.Name, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create product.");
            return View(request);
        }
        TempData[TempDataKeys.SuccessMessage] = ProductMessages.CreateSuccess;
        return RedirectToAction(nameof(Details), new { id = result.Data });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var detail = await _productService.GetByIdAsync(id, cancellationToken);
        if (detail.Data == null)
        {
            TempData[TempDataKeys.ErrorMessage] = ProductMessages.NotFound;
            return RedirectToAction(nameof(Index));
        }
        var req = new UpdateProductRequest
        {
            Id = detail.Data.Id,
            Name = detail.Data.Name,
            Code = detail.Data.Code,
            Description = detail.Data.Description,
            Unit = detail.Data.Unit,
            LowStockThreshold = detail.Data.LowStockThreshold,
            IsActive = detail.Data.IsActive
        };
        return View(req);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(request);
        var result = await _productService.UpdateAsync(request, User.Identity?.Name, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to update product.");
            return View(request);
        }
        TempData[TempDataKeys.SuccessMessage] = ProductMessages.UpdateSuccess;
        return RedirectToAction(nameof(Details), new { id = request.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetByIdAsync(id, cancellationToken);
        if (result.Data == null)
        {
            TempData[TempDataKeys.ErrorMessage] = ProductMessages.NotFound;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await _productService.DeleteAsync(id, cancellationToken);
        if (!result.Success)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error ?? "Failed to delete product.";
            return RedirectToAction(nameof(Index));
        }
        TempData[TempDataKeys.SuccessMessage] = ProductMessages.DeleteSuccess;
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> LowStock(CancellationToken cancellationToken)
    {
        var result = await _productService.GetLowStockAlertsAsync(cancellationToken);
        if (!result.Success)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return View(new List<LowStockAlertResponse>());
        }
        return View(result.Data ?? new List<LowStockAlertResponse>());
    }

    public async Task<IActionResult> MovementHistory(int? productId, int? warehouseId, CancellationToken cancellationToken)
    {
        var result = await _productService.GetMovementHistoryAsync(productId, warehouseId, 100, cancellationToken);
        if (!result.Success)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return View(new List<StockMovementListResponse>());
        }
        return View(result.Data ?? new List<StockMovementListResponse>());
    }
}
