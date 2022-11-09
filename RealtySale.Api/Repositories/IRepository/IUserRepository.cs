using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.IRepository;

public interface IUserRepository
{
    Task<UserRepositoryResponse> AuthenticateAsync(string username, string password);
    Task<UserRepositoryResponse> RegisterAsync(string username, string password);
    Task<UserRepositoryResponse> IsUserExistsAsync(string username);
}