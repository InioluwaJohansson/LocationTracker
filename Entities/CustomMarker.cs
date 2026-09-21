using System.ComponentModel.DataAnnotations;
using LocationTracker.Contracts;
namespace LocationTracker.Entities;
public class CustomMarker : AuditableEntity
{
    [Required]
    public int UserId { get; set; }
    public string UserColor { get; set; } = "#3B82F6";
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "generic";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFromStayPoint { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}