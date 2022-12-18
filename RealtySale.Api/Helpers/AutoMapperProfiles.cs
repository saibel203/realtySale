using AutoMapper;
using RealtySale.Shared.Data;
using RealtySale.Shared.DTOs;

namespace RealtySale.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<City, CityDto>().ReverseMap();
        CreateMap<City, CityUpdateDto>().ReverseMap();
        
        CreateMap<Photo, PhotoDto>().ReverseMap();
        
        CreateMap<Property, PropertyDto>().ReverseMap();
        CreateMap<Property, PropertyListDto>()
            .ForMember(x => x.City, options => options.MapFrom(src => src.City!.Name))
            .ForMember(x => x.Country, options => options.MapFrom(src => src.City!.Country))
            .ForMember(x => x.PropertyType, options => options.MapFrom(src => src.PropertyType!.Name))
            .ForMember(x => x.FurnishingType, options => options.MapFrom(src => src.FurnishingType!.Name))
            .ForMember(x => x.Photo, options => options.MapFrom(src => src.Photos!
                .FirstOrDefault(p => p.IsPrimary)!.ImageUrl));
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(x => x.City, options => options.MapFrom(src => src.City!.Name))
            .ForMember(x => x.Country, options => options.MapFrom(src => src.City!.Country))
            .ForMember(x => x.PropertyType, options => options.MapFrom(src => src.PropertyType!.Name))
            .ForMember(x => x.FurnishingType, options => options.MapFrom(src => src.FurnishingType!.Name));
        
        CreateMap<PropertyType, KeyValuePairDto>();
        CreateMap<FurnishingType, KeyValuePairDto>();
        
        CreateMap<User, UserUpdateDto>().ReverseMap();
        CreateMap<User, UsernameDto>().ReverseMap();
    }
}
