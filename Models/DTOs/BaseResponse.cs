namespace LocationTracker.Models.DTOs;
public class BaseResponse
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}