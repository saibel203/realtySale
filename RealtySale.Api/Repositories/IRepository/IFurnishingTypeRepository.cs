using RealtySale.Shared.Data;

namespace RealtySale.Api.Repositories.IRepository;

public interface IFurnishingTypeRepository
{
    Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync();
}