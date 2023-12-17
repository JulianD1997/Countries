using AutoMapper;
using Countries.Models;
using Countries.Models.Dto;

namespace Countries;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<CountryData, Country>()
            .ForMember(_model => _model.IsoCode, dto => dto.MapFrom( data => data.iso2))
            .ForMember(_model => _model.Name, dto => dto.MapFrom(data => data.country));
        CreateMap<RegisterCountryDto, Country>();
        CreateMap<RegisterHotelDto, Hotel>();
        CreateMap<RegisterRestaurantDto, Restaurant>();
    }
}
