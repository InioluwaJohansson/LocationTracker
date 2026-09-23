using LocationTracker.Models.DTOs;

namespace LocationTracker.Authentication;

public interface IJWTAuthentication
{
    public GetUserDto? GetUserFromToken(string token);
}