namespace biztrack.ostad.Contract.Response;

public class StockByWarehouseResponse
{
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}
