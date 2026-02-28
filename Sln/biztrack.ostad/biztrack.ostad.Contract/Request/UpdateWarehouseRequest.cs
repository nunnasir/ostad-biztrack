using System.ComponentModel.DataAnnotations;

namespace biztrack.ostad.Contract.Request;

public class UpdateWarehouseRequest
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;
}
