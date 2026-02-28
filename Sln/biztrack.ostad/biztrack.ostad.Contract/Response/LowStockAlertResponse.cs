namespace biztrack.ostad.Contract.Response;

public class LowStockAlertResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductCode { get; set; }
    public decimal CurrentStock { get; set; }
    public int? LowStockThreshold { get; set; }
}
