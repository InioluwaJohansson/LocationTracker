using System.Text.Json;
using LocationTracker.Authentication;
using LocationTracker.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Route("LocationTracker/[controller]")]
[ApiController]
public class UserController : Controller
{
    ICoordinateService _coordinateService;
    IJWTAuthentication _jwtAuthentication;
    public IConfiguration _baseUrl;
    public UserController(ICoordinateService coordinateService, IJWTAuthentication jwtAuthentication, IConfiguration baseUrl)
    {
        _jwtAuthentication = jwtAuthentication;
        _coordinateService = coordinateService;
        _baseUrl = baseUrl.GetSection("BaseApp");
    }
    [HttpGet("Login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"{_baseUrl["BaseUrl"]}{_baseUrl["LoginUrl"]}userName={username}&password={password}");
        response.EnsureSuccessStatusCode();
        return Ok(await response.Content.ReadAsStringAsync());
    }
    [Authorize]
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "") ?? "";
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user == null) return Unauthorized();
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.PostAsync($"{_baseUrl["BaseUrl"]}{_baseUrl["GetAllUsersUrl"]}", null);
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var personIdNames = document.RootElement.GetProperty("personIdNames");
        return Content(personIdNames.GetRawText(),"application/json");
    }
    [Authorize]
    [HttpGet("GetUserLocation")]
    public async Task<IActionResult> GetUserLocation(int userId)
    {
        var data = await _coordinateService.GetUserLastCoordinates(userId);
        return Ok(data);
    }
}