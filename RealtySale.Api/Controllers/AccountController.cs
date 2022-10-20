using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealtySale.Api.Data.IRepositories;
using RealtySale.Api.DTOs;
using RealtySale.Api.Models;

namespace RealtySale.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AccountController(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    [HttpPost("login")] // /api/account/login
    public async Task<IActionResult> Login(LoginReqDto loginReq)
    {
        var user = await _unitOfWork.UserRepository.AuthenticateAsync(loginReq.Username, loginReq.Password);
        if (user is null)
            return Unauthorized();
        var response = new LoginResDto();
        response.Username = user.Username;
        response.Token = GenerateJwtToken(user);
        return Ok(response);
    }

    private string GenerateJwtToken(User user)
    {
        var key = _configuration["JwtOptions:Key"];
        var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var signingCredentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = signingCredentials,
            Issuer = _configuration["JwtOptions:Issuer"],
            Audience = _configuration["JwtOptions:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
