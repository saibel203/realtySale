using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;

namespace RealtySale.Api.Repositories.Repository;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealtySaleContext _context;

    public PropertyRepository(RealtySaleContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetPropertiesAsync(byte sellRent)
    {
        var properties = await _context.Properties.Where(type => type.SellRent == sellRent)
            .Include(pt => pt.PropertyType)
            .Include(pc => pc.City)
            .Include(pf => pf.FurnishingType)
            .ToListAsync();
        return properties;
    }

    public async Task<Property> GetPropertyDetailsAsync(long id)
    {
        var property = await _context.Properties
            .Include(pt => pt.PropertyType)
            .Include(pc => pc.City)
            .Include(pf => pf.FurnishingType)
            .Include(pp => pp.Photos)
            .Where(x => x.Id == id)
            .FirstAsync();
        return property;
    }

    public async Task<Property> GetPropertyByIdAsync(long id)
    {
        var property = await _context.Properties
            .Include(pp => pp.Photos)
            .Where(x => x.Id == id)
            .FirstAsync();

        return property;
    }

    public async Task AddPropertyAsync(Property? property)
    {
        if (property is null)
            throw new NullReferenceException();
        
        await _context.Properties.AddAsync(property);
    }
}
