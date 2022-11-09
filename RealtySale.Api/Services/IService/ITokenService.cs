using RealtySale.Shared.Data;

namespace RealtySale.Api.Services.IService;

public interface ITokenService
{
    string GenerateToken(User userDto);
}