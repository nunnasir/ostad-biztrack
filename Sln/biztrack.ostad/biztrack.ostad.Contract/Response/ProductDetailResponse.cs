namespace biztrack.ostad.Contract.Response;

public class ProductDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public int? LowStockThreshold { get; set; }
    public bool IsActive { get; set; }
    public decimal TotalStock { get; set; }
    public bool IsLowStock { get; set; }
    public List<StockByWarehouseResponse> StockByWarehouse { get; set; } = new();
}
