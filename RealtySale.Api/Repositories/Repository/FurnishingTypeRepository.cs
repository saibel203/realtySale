using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.Repository;

public class FurnishingTypeRepository : IFurnishingTypeRepository
{
    private readonly RealtySaleContext _context;

    public FurnishingTypeRepository(RealtySaleContext context)
    {
        _context = context;
    }
    
    public async Task<FurnishingTypeRepositoryResponse> GetFurnishingTypesAsync()
    {
        var furnishingTypes = await _context.FurnishingTypes.ToListAsync();
        
        return new()
        {
            Message = "Furnishing types successfully get",
            IsSuccess = true,
            FurnishingTypes = furnishingTypes
        };
    }
}
