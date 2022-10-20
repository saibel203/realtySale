using AutoMapper;
using RealtySale.Api.DTOs;
using RealtySale.Api.Models;

namespace RealtySale.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<City, CityDto>().ReverseMap();
        CreateMap<City, CityUpdateDto>().ReverseMap();
    }
}
