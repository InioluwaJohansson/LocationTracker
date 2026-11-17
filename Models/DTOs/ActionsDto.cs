namespace LocationTracker.Models.DTOs;
public class CreateActionDto
{
    public string ActionName { get; set; }
    public double DistanceToMarker { get; set; }
    public bool IsActive { get; set; }
    public int CustomMarkerId { get; set; }
    public List<int> ActionIds { get; set; }
}
public class UpdateActionDto
{
    public string ActionName { get; set; }
    public double DistanceToMarker { get; set; }
    public int CustomMarkerId { get; set; }
    public List<int> ActionIds { get; set; }
}
public class GetActionDto
{
    public string ActionName { get; set; }
    public double DistanceToMarker { get; set; }
    public bool IsActive { get; set; }
    public int CustomMarkerId { get; set; }
    public List<int> ActionIds { get; set; }
}
public class ActionResponseModel : BaseResponse
{
    public GetActionDto Data { get; set; }
}
public class ActionsResponseModel : BaseResponse
{
    public ICollection<GetActionDto> Data { get; set; } = new HashSet<GetActionDto>();
}