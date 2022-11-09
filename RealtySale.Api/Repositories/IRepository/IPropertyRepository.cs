using RealtySale.Shared.Data;

namespace RealtySale.Api.Repositories.IRepository;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetPropertiesAsync(byte sellRent);
    Task<Property> GetPropertyDetailsAsync(long id);
    Task<Property> GetPropertyByIdAsync(long id);
    Task AddPropertyAsync(Property? property);
}