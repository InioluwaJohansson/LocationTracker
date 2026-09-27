using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using LocationTracker.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace LocationTracker.Authentication;

public class JWTAuthentication : IJWTAuthentication
{
    public IConfiguration _key;
    public IAuthCache _authCache;
    public JWTAuthentication(IConfiguration config, IAuthCache authCache)
    {
        _key = config.GetSection("Jwt");
        _authCache = authCache;
    }
    public GetUserDto? GetUserFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_authCache._credentials.SecretKey ?? "HomeSecurity1234567890");

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _authCache._credentials.Issuer ?? "Home_Security",
            ValidAudience = _authCache._credentials.Audience ?? "Home",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            var claims = principal.Claims;
            var timeStamp = DateTime.Parse(claims.First(x => x.Type == "TimeStamp").Value);
            if(timeStamp.AddDays(1) < DateTime.Now) return null;
            if(int.Parse(claims.First(x => x.Type == "UserId").Value) > 0 && !_authCache._jwtToken.Contains(token)) _authCache._jwtToken.Add(token);
            return new GetUserDto
            {
                Id = int.Parse(claims.First(x => x.Type == "UserId").Value),
                PersonId = int.Parse(claims.First(x => x.Type == "PersonId").Value),
                UserName = claims.First(x => x.Type == "UserName").Value,
                RoleName = claims.First(x => x.Type == "RoleName").Value,
            };
        }
        catch
        {
            return null;
        }
    }
    public async Task RefreshTokenFromExternalApi(string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync("https://localhost:7190/Home_Security/User/RefreshToken");
        response.EnsureSuccessStatusCode();        
        if(response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var loginJson = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(loginJson);
            var refreshedUser = GetUserFromToken(jsonDocument.RootElement.GetProperty("token").GetString());
            var user = GetUserFromToken(token);
            if (refreshedUser != null && _authCache._jwtToken.Contains(token))
            {
                _authCache._jwtToken.Remove(token);
                _authCache._userCache.Remove(user);
                _authCache._jwtToken.Add(jsonDocument.RootElement.GetProperty("token").GetString());
                _authCache._userCache.Add(refreshedUser);
            }
        }
    }
    public async Task RefreshAllTokens()
    {
        foreach (var token in _authCache._jwtToken.ToList())
        {
            await RefreshTokenFromExternalApi(token);
        }
    }
    public async Task GetSigningData()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "LocationTracker");
        var url = "https://localhost:7190/Home_Security/User/GetSigningCredentials";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var credentials =  await response.Content.ReadFromJsonAsync<Credentials>();
        if (credentials != null)
        {
            _authCache._credentials.SecretKey = credentials.SecretKey;
            _authCache._credentials.Token = credentials.Token;
            _authCache._credentials.Issuer = credentials.Issuer;
            _authCache._credentials.Audience = credentials.Audience;
        }
    }
}