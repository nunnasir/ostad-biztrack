using System.ComponentModel.DataAnnotations;

namespace biztrack.ostad.Contract.Request;

public class RecordStockMovementRequest
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    /// <summary>Positive for in, negative for out.</summary>
    public decimal Quantity { get; set; }
    public StockMovementType MovementType { get; set; }
    [MaxLength(200)]
    public string? Reference { get; set; }
}
