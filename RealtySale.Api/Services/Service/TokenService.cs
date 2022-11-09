using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.Data;

namespace RealtySale.Api.Services.Service;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string GenerateToken(User userDto)
    {
        var key = _configuration["JwtOptions:Key"];
        var issuer = _configuration["JwtOptions:Issuer"];
        var audience = _configuration["JwtOptions:Audience"];
        var expires = DateTime.UtcNow.AddDays(10);
        var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userDto.Username),
            new(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        var credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
            expires: expires, signingCredentials: credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var resultToken = tokenHandler.WriteToken(tokenDescriptor);

        return resultToken;
    }
}
