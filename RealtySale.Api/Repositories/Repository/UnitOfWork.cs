using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;

namespace RealtySale.Api.Repositories.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly RealtySaleContext _context;

    public UnitOfWork(RealtySaleContext context)
    {
        _context = context;
    }

    public ICityRepository CityRepository => new CityRepository(_context);
    public IUserRepository UserRepository => new UserRepository(_context);
    public IPropertyRepository PropertyRepository => new PropertyRepository(_context);
    public IPropertyTypeRepository PropertyTypeRepository => new PropertyTypeRepository(_context);
    public IFurnishingTypeRepository FurnishingTypeRepository => new FurnishingTypeRepository(_context);
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
