using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.IRepository;

public interface IPropertyTypeRepository
{
    Task<PropertyTypeRepositoryResponse> GetPropertyTypesAsync();
}