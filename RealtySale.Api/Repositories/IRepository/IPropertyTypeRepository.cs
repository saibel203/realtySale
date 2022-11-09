using RealtySale.Shared.Data;

namespace RealtySale.Api.Repositories.IRepository;

public interface IPropertyTypeRepository
{
    Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
}