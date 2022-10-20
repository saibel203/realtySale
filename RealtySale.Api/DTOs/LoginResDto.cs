using System.Runtime.CompilerServices;

namespace RealtySale.Api.DTOs;

public class LoginResDto
{
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
