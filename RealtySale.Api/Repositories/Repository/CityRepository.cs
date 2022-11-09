using Microsoft.EntityFrameworkCore;
using RealtySale.Api.Models;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Repositories.Repository;

public class CityRepository : ICityRepository
{
    private readonly RealtySaleContext _context;

    public CityRepository(RealtySaleContext context)
    {
        _context = context;
    }
    
    public async Task<CityRepositoryResponse> GetCityAsync(long id)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);

        if (city is null)
            return new()
            {
                Message = "City not found",
                IsSuccess = false
            };

        return new()
        {
            Message = "City was found",
            IsSuccess = true,
            City = city
        };
    }

    public async Task<CityRepositoryResponse> GetAllCitiesAsync()
    {
        IEnumerable<City> cities = await _context.Cities.ToListAsync();

        return new()
        {
            Message = "All cities list",
            IsSuccess = true,
            CitiesList = cities
        };
    }

    public async Task<CityRepositoryResponse> NewCityAsync(City? city)
    {
        if (city is not null)
        {
            await _context.Cities.AddAsync(city);
            return new()
            {
                Message = "City added successfully",
                IsSuccess = true
            };
        }

        return new()
        {
            Message = "City has Null value or invalid some properties",
            IsSuccess = false
        };
    }

    public async Task<CityRepositoryResponse> DeleteCityAsync(long id)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);

        if (city is null)
            return new()
            {
                Message = "City not found!",
                IsSuccess = false
            };

        _context.Cities.Remove(city);

        return new()
        {
            Message = "City delete successful",
            IsSuccess = true
        };
    }
}
