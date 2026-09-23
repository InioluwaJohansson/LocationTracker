using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LocationTracker.Models.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace LocationTracker.Authentication;

public class JWTAuthentication : IJWTAuthentication
{
    public IConfiguration _key;
    public JWTAuthentication(IConfiguration config)
    {
        _key = config.GetSection("Jwt");
    }
    public GetUserDto? GetUserFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_key["SecretKey"] ?? "HomeSecurity1234567890");

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _key["Issuer"] ?? "Home_Security",
            ValidAudience = _key["Audience"] ?? "Home",

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
            return new GetUserDto
            {
                Id = int.Parse(claims.First(x => x.Type == "UserId").Value),
                PersonId = int.Parse(claims.First(x => x.Type == "PersonId").Value),
                UserName = claims.First(x => x.Type == "UserName").Value,
                RoleName = claims.First(x => x.Type == "RoleName").Value,
                AuthorizationCode = claims.First(x => x.Type == "AuthorizationCode").Value
            };
        }
        catch
        {
            return null;
        }
    }
}