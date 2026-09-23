using LocationTracker.Models.DTOs;

namespace LocationTracker.Authentication;
public interface IAuthCache
{
    public List<GetUserDto> _userCache { get; }
    public List<string> _jwtToken { get; }
    public List<Credentials> _credentials { get; }
}