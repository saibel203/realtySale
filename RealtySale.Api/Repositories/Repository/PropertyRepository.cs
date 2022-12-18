using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.Repository;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealtySaleContext _context;

    public PropertyRepository(RealtySaleContext context)
    {
        _context = context;
    }

    public async Task<PropertyRepositoryResponse> GetPropertiesAsync(byte sellRent)
    {
        var properties = await _context.Properties.Where(type => type.SellRent == sellRent)
            .Include(pt => pt.PropertyType)
            .Include(pc => pc.City)
            .Include(pc => pc.Photos)
            .Include(pf => pf.FurnishingType)
            .ToListAsync();
        return new()
        {
            Message = "Properties successfully get",
            IsSuccess = true,
            Properties = properties
        };
    }

    public async Task<PropertyRepositoryResponse> GetPropertyDetailsAsync(long id)
    {
        var property = await _context.Properties
            .Include(pt => pt.PropertyType)
            .Include(pc => pc.City)
            .Include(pf => pf.FurnishingType)
            .Include(pp => pp.Photos)
            .Where(x => x.Id == id)
            .FirstAsync();
        return new()
        {
            Message = "Property get successfully",
            IsSuccess = true,
            Property = property
        };
    }

    public async Task<PropertyRepositoryResponse> GetPropertyByIdAsync(long id)
    {
        var property = await _context.Properties
            .Include(pp => pp.Photos)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        return new()
        {
            Message = "Property get successfully",
            IsSuccess = true,
            Property = property
        };
    }

    public async Task<PropertyRepositoryResponse> AddPropertyAsync(Property? property)
    {
        if (property is null)
            return new()
            {
                Message = "Property has some errors",
                IsSuccess = false
            };

        await _context.Properties.AddAsync(property);
        
        return new()
        {
            Message = "Property add success",
            IsSuccess = true
        };
    }

    public async Task<PropertyRepositoryResponse> GetUserPropertiesAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (user is null)
            return new()
            {
                Message = "User not found",
                IsSuccess = false
            };

        var properties = await _context.Properties.Where(x => x.PostedBy == user.Id).ToListAsync();

        return new()
        {
            Message = "Successfully get all user properties",
            IsSuccess = true,
            Properties = properties
        };
    }

    public async Task<PropertyRepositoryResponse> GetFavouriteListAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (user is null)
            return new()
            {
                Message = "User not found",
                IsSuccess = false
            };
        
        var properties = await _context.FavouriteProperties.Include(x => x.User)
            .Where(x => x.UserId == user.Id).Select(x => x.Property).ToListAsync();

        return new()
        {
            Message = "Properties received successfully",
            IsSuccess = true,
            Properties = properties!
        };
    }
}
