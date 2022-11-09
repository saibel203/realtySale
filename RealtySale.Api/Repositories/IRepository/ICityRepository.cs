using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.IRepository;

public interface ICityRepository
{
    Task<CityRepositoryResponse> GetCityAsync(long id);
    Task<CityRepositoryResponse> GetAllCitiesAsync();
    Task<CityRepositoryResponse> NewCityAsync(City? city);
    Task<CityRepositoryResponse> DeleteCityAsync(long id);
}