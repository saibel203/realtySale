using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.IRepository;

public interface IFurnishingTypeRepository
{
    Task<FurnishingTypeRepositoryResponse> GetFurnishingTypesAsync();
}