namespace biztrack.ostad.Domain;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    /// <summary>When stock falls at or below this value, consider it low stock.</summary>
    public int? LowStockThreshold { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
