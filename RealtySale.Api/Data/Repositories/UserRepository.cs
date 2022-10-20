using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Data.IRepositories;
using RealtySale.Api.Models;

namespace RealtySale.Api.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RealtySaleContext _context;

    public UserRepository(RealtySaleContext context)
    {
        _context = context;
    }
    
    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        
        return user;
    }
}
