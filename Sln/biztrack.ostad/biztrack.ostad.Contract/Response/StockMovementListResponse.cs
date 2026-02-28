namespace biztrack.ostad.Contract.Response;

public class StockMovementListResponse
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public DateTime MovementDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
