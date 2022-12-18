using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.Repository;

public class PropertyTypeRepository : IPropertyTypeRepository
{
    private readonly RealtySaleContext _context;

    public PropertyTypeRepository(RealtySaleContext context)
    {
        _context = context;
    }

    public async Task<PropertyTypeRepositoryResponse> GetPropertyTypesAsync()
    {
        var propertyTypes = await _context.PropertyTypes.ToListAsync();

        return new()
        {
            Message = "Property types successfully get",
            IsSuccess = true,
            PropertyTypes = propertyTypes
        };
    }
}
