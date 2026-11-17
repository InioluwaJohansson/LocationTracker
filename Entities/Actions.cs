using System.ComponentModel.DataAnnotations;
using LocationTracker.Contracts;
namespace LocationTracker.Entities;
public class Actions : AuditableEntity
{
    public string ActionName { get; set; }
    public double DistanceToMarker { get; set; }
    public bool IsActive { get; set; }
    public int CustomMarkerId { get; set; }
    public CustomMarker CustomMarker { get; set; }
    public List<int> ActionIds { get; set; }
}