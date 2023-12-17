using Countries.Models;
using Countries.Models.Dto;

namespace Countries.Services.Interfaces;

public interface ICountryService
{
    Task<Detail> Save(RegisterCountryDto country);
    Task<Detail> UpdateList();
    Task<Detail> GetAll();
    Task<Detail> GetContries(int page, int pageSize);
    Task<Detail> Update(UpdateCountryDto updateCountry);
    Task<Detail> Delete(string name);
    Task<Detail> AddRestaurants(AddObjectToCountry countryRestaurant);
    Task<Detail> AddHotels(AddObjectToCountry countryHotel);
}
