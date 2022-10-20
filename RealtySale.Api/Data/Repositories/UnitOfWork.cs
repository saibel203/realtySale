using RealtySale.Api.Data.IRepositories;
using RealtySale.Api.Models;

namespace RealtySale.Api.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RealtySaleContext _context;

    public UnitOfWork(RealtySaleContext context)
    {
        _context = context;
    }

    public ICityRepository CityRepository => new CityRepository(_context);
    public IUserRepository UserRepository => new UserRepository(_context);
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
