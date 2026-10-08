using Microsoft.AspNetCore.Mvc;
[Route("LocationTracker/[controller]")]
[ApiController]
public class UserController : Controller
{
    public IConfiguration _baseUrl;
    public UserController(IConfiguration baseUrl)
    {
        _baseUrl = baseUrl.GetSection("BaseApp");
    }
    [HttpGet("Login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"{_baseUrl["BaseUrl"]}{_baseUrl["LoginUrl"]}?userName={username}&password={password}");
        response.EnsureSuccessStatusCode();
        return Ok(await response.Content.ReadAsStringAsync());
    }
}