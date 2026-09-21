using System.ComponentModel.DataAnnotations;

namespace LocationTracker.Models.DTOs;
public class CreateCustomMarkerDto
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
}
public class GetCustomMarkerDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserColor { get; set; } = "#3B82F6";
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "generic";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFromStayPoint { get; set; }
    public DateTimeOffset CreatedAt { get; set; } 
}
public class UpdateCustomMarkerDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserColor { get; set; } = "#3B82F6";
    [Required] 
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "generic";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFromStayPoint { get; set; }
}