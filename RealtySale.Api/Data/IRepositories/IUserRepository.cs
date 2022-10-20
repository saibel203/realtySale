using RealtySale.Api.Models;

namespace RealtySale.Api.Data.IRepositories;

public interface IUserRepository
{
    Task<User> AuthenticateAsync(string username, string password);
}