using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("storage")]
public class StorageController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public StorageController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration.GetSection("BaseApp");
    }
    [HttpGet("{**path}")]
    public async Task<IActionResult> GetFile(string path)
    {
        var url = $"{_configuration["BaseUrl"]}{_configuration["StorageUrl"]}/{path}";
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
        return File(stream, contentType);
    }
}