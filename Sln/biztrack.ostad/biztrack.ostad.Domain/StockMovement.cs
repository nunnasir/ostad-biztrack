namespace biztrack.ostad.Domain;

public enum StockMovementType
{
    In = 0,
    Out = 1,
    Adjustment = 2
}

public class StockMovement : Entity
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    /// <summary>Positive for in, negative for out.</summary>
    public decimal Quantity { get; set; }
    public StockMovementType MovementType { get; set; }
    public string? Reference { get; set; }
    public DateTime MovementDate { get; set; }

    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}
