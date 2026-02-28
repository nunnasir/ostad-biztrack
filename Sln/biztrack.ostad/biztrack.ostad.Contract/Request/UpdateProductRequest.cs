using System.ComponentModel.DataAnnotations;

namespace biztrack.ostad.Contract.Request;

public class UpdateProductRequest
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Unit { get; set; }

    public int? LowStockThreshold { get; set; }

    public bool IsActive { get; set; } = true;
}
