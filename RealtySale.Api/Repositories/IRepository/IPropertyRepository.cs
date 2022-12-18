using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.IRepository;

public interface IPropertyRepository
{
    Task<PropertyRepositoryResponse> GetPropertiesAsync(byte sellRent);
    Task<PropertyRepositoryResponse> GetPropertyDetailsAsync(long id);
    Task<PropertyRepositoryResponse> GetPropertyByIdAsync(long id);
    Task<PropertyRepositoryResponse> AddPropertyAsync(Property? property);
    Task<PropertyRepositoryResponse> GetUserPropertiesAsync(string username);
    Task<PropertyRepositoryResponse> GetFavouriteListAsync(string username);
}