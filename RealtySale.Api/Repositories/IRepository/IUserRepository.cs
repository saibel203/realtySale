using RealtySale.Shared.DTOs;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.IRepository;

public interface IUserRepository
{
    Task<UserRepositoryResponse> AuthenticateAsync(string username, string password);
    Task<UserRepositoryResponse> RegisterAsync(string username, string password, string email);
    Task<UserRepositoryResponse> IsUserExistsAsync(string username);
    Task<UserRepositoryResponse> ChangePasswordAsync(string username, string oldPassword, string newPassword);
    Task<UserRepositoryResponse> GetUserDataAsync(string username);
    Task<UserRepositoryResponse> GetUserDataAsync(long id);
    Task<UserRepositoryResponse> AddFavouritePropertyAsync(UsernameDto user, int id);
    Task<UserRepositoryResponse> IsPropertyFavouriteAsync(UsernameDto user, int id);
}