using AutoMapper;
using Countries.Data;
using Countries.Models;
using Countries.Models.Dto;
using Countries.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Countries.Services;

public class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    private readonly IRestCountriesService _restCountriesService;
    private readonly CountryDbContext _dbContext;
    public CountryService(IMapper mapper, IRestCountriesService restCountriesService, CountryDbContext dbContext)
    {
        _mapper = mapper;
        _restCountriesService = restCountriesService;
        _dbContext = dbContext;
    }
    public async Task<Detail> UpdateList()
    {
        var countriesData = await _restCountriesService.GetAllCountries();
        foreach (var country in countriesData)
        {
            Console.WriteLine(country.Name);
            var findCountry = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Name.ToLower() == country.Name.ToLower());
            if (findCountry == null) {
                await _dbContext.Countries.AddAsync(country);
                await _dbContext.SaveChangesAsync();
            }
        }
        return new Detail 
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = "La lista de paises ha sido actualizada."
        };
    }

    public async Task<Detail> Save(RegisterCountryDto country)
    {
        var findCountry = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Name.ToLower() == country.Name.ToLower());
        if (findCountry == null)
        {
            var newCountry = _mapper.Map<Country>(country);
            await _dbContext.Countries.AddAsync(newCountry);
            await _dbContext.SaveChangesAsync();
            return new Detail 
            {
                Status = ResponseStatus.Success,
                IsSuccessful = true,
                Message = $"{newCountry.Name} fue guardado correctamente."
            };
        }
        return new Detail
        {
            Status = ResponseStatus.BadRequest,
            IsSuccessful = false,
            Message = $"El país {country.Name} ya existe."
        };
    }

    public async Task<Detail> GetAll()
    {
        var listCountries = await _dbContext.Countries
            .Include(c => c.CountryRestaurants)
            .ThenInclude(cr => cr.Restaurant)
            .Include(c => c.CountryHotels)
            .ThenInclude(ch => ch.Hotel).ToListAsync();
        if (!listCountries.Any()) {
            return new Detail
            {
                Status = ResponseStatus.NoContent,
                IsSuccessful = false,
                Message = "No hay datos para mostrar."
            };
        }
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = "Lista de países.",
            Countries = listCountries
        };
    }
    public async Task<Detail> Update(UpdateCountryDto updateCountry)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Id == updateCountry.Id);
        if (country == null)
        {
            return new Detail
            {
                Status = ResponseStatus.NotFound,
                IsSuccessful = false,
                Message = $"El país {updateCountry.Name} no existe en la base de datos."
            };
        }
        country.Name = updateCountry.Name ?? country.Name;
        country.IsoCode = updateCountry.IsoCode ?? country.IsoCode;
        _dbContext.Countries.Update(country);
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"{updateCountry.Name} actualizado correctamente."
        };
    }
    public async Task<Detail> Delete(string name)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        if (country == null)
        {
            return new Detail
            {
                Status = ResponseStatus.NotFound,
                IsSuccessful = false,
                Message = $"El país {name} no existe en la base de datos."
            };
        }
        _dbContext.Countries.Remove(country);
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"{name} eliminado correctamente."
        };
    }
    public async Task<Detail> AddHotels(AddObjectToCountry countryHotel)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Name.ToLower() == countryHotel.CountryName.ToLower());
        if (country == null)
        {
            return new Detail
            {
                Status = ResponseStatus.BadRequest,
                IsSuccessful = false,
                Message = $"El país {countryHotel.CountryName} no existe."
            };
        }
        foreach (int id in countryHotel.Ids)
        {
            var hotel = await _dbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return new Detail
                {
                    Status = ResponseStatus.BadRequest,
                    IsSuccessful = false,
                    Message = $"El id {id} no existe en la tabla Hotels."
                };
            }
            country.CountryHotels.Add(new CountryHotel()
            {
                CountryId = country.Id,
                Country = country,
                HotelId = id,
                Hotel = hotel
            });
        }
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.BadRequest,
            IsSuccessful = false,
            Message = $"Los hoteles fueron agregados correctamente."
        };
    }
    public async Task<Detail> AddRestaurants(AddObjectToCountry countryRestaurant)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Name.ToLower() == countryRestaurant.CountryName.ToLower());
        if (country == null)
        {
            return new Detail
            {
                Status = ResponseStatus.BadRequest,
                IsSuccessful = false,
                Message = $"El país {countryRestaurant.CountryName} no existe."
            };
        }
        foreach (int id in countryRestaurant.Ids)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(h => h.Id == id);
            if (restaurant == null)
            {
                return new Detail
                {
                    Status = ResponseStatus.BadRequest,
                    IsSuccessful = false,
                    Message = $"El id {id} no existe en la tabla Restaurants."
                };
            }
            country.CountryRestaurants.Add(new CountryRestaurant()
            {
                CountryId = country.Id,
                Country = country,
                RestaurantId = id,
                Restaurant = restaurant
            });
        }
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.BadRequest,
            IsSuccessful = false,
            Message = $"Los restaurantes fueron agregados correctamente."
        };
    }
}
