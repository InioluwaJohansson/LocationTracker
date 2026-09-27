using LocationTracker.Models.DTOs;

namespace LocationTracker.Authentication;
public class AuthCache : IAuthCache
{
    public List<string> _jwtToken { get; } = new();
    public List<GetUserDto> _userCache { get; } =  new ();
    public Credentials _credentials { get; } =  new ();
}