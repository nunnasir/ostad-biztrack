namespace biztrack.ostad.Domain;

/// <summary>
/// Base entity with audit fields and integer primary key.
/// </summary>
public abstract class Entity
{
    public int Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
