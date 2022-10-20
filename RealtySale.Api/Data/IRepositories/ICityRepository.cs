using RealtySale.Api.Models;
using RealtySale.Shared;

namespace RealtySale.Api.Data.IRepositories;

public interface ICityRepository
{
    Task<CityRepositoryResponse> GetCityAsync(long id);
    Task<CityRepositoryResponse> GetAllCitiesAsync();
    Task<CityRepositoryResponse> NewCityAsync(City? city);
    Task<CityRepositoryResponse> DeleteCityAsync(long id);
}