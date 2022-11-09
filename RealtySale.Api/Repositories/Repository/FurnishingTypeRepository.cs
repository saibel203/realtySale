using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;

namespace RealtySale.Api.Repositories.Repository;

public class FurnishingTypeRepository : IFurnishingTypeRepository
{
    private readonly RealtySaleContext _context;

    public FurnishingTypeRepository(RealtySaleContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync()
    {
        var furnishingTypes = await _context.FurnishingTypes.ToListAsync();
        return furnishingTypes;
    }
}
