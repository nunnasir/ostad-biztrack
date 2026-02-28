using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using biztrack.ostad.Application.Interfaces;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Contract.Response;
using biztrack.ostad.Web.Constants;

namespace biztrack.ostad.Web.Controllers;

[Authorize]
public class WarehousesController : Controller
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    public async Task<IActionResult> Index(bool activeOnly = false, CancellationToken cancellationToken = default)
    {
        var result = await _warehouseService.GetAllAsync(activeOnly, cancellationToken);
        if (!result.Success)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return View(new List<WarehouseListResponse>());
        }
        return View(result.Data ?? new List<WarehouseListResponse>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateWarehouseRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(request);
        var result = await _warehouseService.CreateAsync(request, User.Identity?.Name, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create warehouse.");
            return View(request);
        }
        TempData[TempDataKeys.SuccessMessage] = WarehouseMessages.CreateSuccess;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var result = await _warehouseService.GetByIdAsync(id, cancellationToken);
        if (result.Data == null)
        {
            TempData[TempDataKeys.ErrorMessage] = WarehouseMessages.NotFound;
            return RedirectToAction(nameof(Index));
        }
        var req = new UpdateWarehouseRequest
        {
            Id = result.Data.Id,
            Name = result.Data.Name,
            Code = result.Data.Code,
            Address = result.Data.Address,
            IsActive = result.Data.IsActive
        };
        return View(req);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(request);
        var result = await _warehouseService.UpdateAsync(request, User.Identity?.Name, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to update warehouse.");
            return View(request);
        }
        TempData[TempDataKeys.SuccessMessage] = WarehouseMessages.UpdateSuccess;
        return RedirectToAction(nameof(Index));
    }
}
