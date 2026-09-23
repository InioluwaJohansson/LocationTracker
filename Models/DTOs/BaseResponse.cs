namespace LocationTracker.Models.DTOs;
public class BaseResponse
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
public class GetUserDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string UserName { get; set; }
    public string RoleName { get; set; }
    public string AuthorizationCode { get; set; }
}
public class Credentials
{
    public string SecretKey { get; set; }
    public string Tomen { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}