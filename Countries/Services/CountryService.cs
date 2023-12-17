using AutoMapper;
using Countries.Data;
using Countries.Models;
using Countries.Models.Dto;
using Countries.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Countries.Services;

public class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    private readonly IRestCountriesService _restCountriesService;
    private readonly CountryDbContext _dbContext;
    private List<Country> _countries = new List<Country>();
    //Inicializacion de servicios
    public CountryService(IMapper mapper, IRestCountriesService restCountriesService, CountryDbContext dbContext)
    {
        _mapper = mapper;
        _restCountriesService = restCountriesService;
        _dbContext = dbContext;
    }
    //actualizar lista de paises por medio de un API
    public async Task<Detail> UpdateList()
    {
        var countriesData = await _restCountriesService.GetAllCountries();
        foreach (var country in countriesData)
        {
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
    //guardar un pais nuevo
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
    //obtener la lista completa de paises
    public async Task<Detail> GetAll()
    {
        var listCountries = await _dbContext.Countries
            .Include(r => r.Restaurants)
            .Include(h => h.Hotels)
            .OrderBy(c => c.Name).ToListAsync();

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
    //Obtener lista de paises por medio de paginacion y tamaño deseado de datos
    public async Task<Detail> GetContries(int page, int pageSize)
    {
        //obtener el total de datos en la base de datos
        var totalCount = await _dbContext.Countries.CountAsync();
        //busqueda de paginacion ordena primero por el nombre
        //salta x pagina x por el tamaño
        //toma x numero de paises
        //incluye relaciones
        var countryPerpage = await _dbContext.Countries
            .OrderBy(c=> c.Name)
            .Skip((page -1) * pageSize)
            .Take(pageSize)
            .Include(r => r.Restaurants)
            .Include(h => h.Hotels)
            .ToListAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"Objetos totales : {totalCount}",
            Countries = countryPerpage
        };
    }
    //actualizar caracteristicas del pais
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
    //borrar un pais por medio de su nombre
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
   //agregar una lista de hoteles al pais
    public async Task<Detail> AddHotels(AddObjectToCountry countryHotel)
    {
        //inicializa la lista de relaciones
        var country = await _dbContext.Countries.Include(ch => ch.CountryHotels)
            .FirstOrDefaultAsync(c => c.Name.ToLower() == countryHotel.CountryName.ToLower());
        if (country == null)
        {
            return new Detail
            {
                Status = ResponseStatus.BadRequest,
                IsSuccessful = false,
                Message = $"El país {countryHotel.CountryName} no existe."
            };
        }
        //recorre la lista de ids
        foreach (int id in countryHotel.Ids)
        {
            //busca el id exista
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
            //agrega la relacion entre el pais y el hotel
            country.CountryHotels.Add(new CountryHotel()
            {
                CountryId = country.Id,
                HotelId = id,
            });
        }
        //guarda cambios
        await _dbContext.SaveChangesAsync();

        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"Los hoteles fueron agregados correctamente."
        };
    }
    //agregar una lista de restaurantes al pais
    public async Task<Detail> AddRestaurants(AddObjectToCountry countryRestaurant)
    {
        var country = await _dbContext.Countries.Include(cr => cr.CountryRestaurants)
            .FirstOrDefaultAsync(c => c.Name.ToLower() == countryRestaurant.CountryName.ToLower());
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
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
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
                RestaurantId = id
            });
        }
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.BadRequest,
            IsSuccessful = true,
            Message = $"Los restaurantes fueron agregados correctamente."
        };
    }
    //metodo para filtrar y buscar contenido
    public async Task<Detail> filterObject(
        string country,
        string iso,
        string restaurant,
        string type,
        string hotel,
        string starts)
    {
        //ser deja como una lista consultable
        var query = _dbContext.Countries.AsQueryable();
        //Condiciones para busqueda
        if (country != null)
        {
            //busqueda por medio del nombre del pais
            query = query.Where(c => c.Name.ToLower().Contains(country.ToLower()));
        }
        if (iso != null)
        {
            //busqueda por medio del codigo Iso
            query = query.Where(c => c.IsoCode.ToLower().Contains(iso.ToLower()));
        }
        //se incluye las relaciones hoteles y restaurantes
        query = query.Include(c => c.Restaurants).Include(c => c.Hotels);

        if (restaurant != null)
        {
            //busqueda por medio del nombre del restaurante
            query = query.Where(c => c.Restaurants.Any(r => r.Name.ToLower().Contains(restaurant.ToLower())));
        }
        if (type != null)
        {
            //busqueda por medio del tipo de restaurante
            query = query.Where(c => c.Restaurants.Any(r => r.Type.ToLower().Contains(type.ToLower())));
        }
        if (hotel != null)
        {
            //busqueda por medio del nombre del hotel
            query = query.Where(c => c.Hotels.Any(r => r.Name.ToLower().Contains(hotel.ToLower())));
        }
        if (starts != null)
        {
            //estrellas del hotel
            query = query.Where(c => c.Hotels.Any(r => r.Starts.ToLower().Contains(starts.ToLower())));
        }
        //se convierte en lista la query resultante
        var filter = await query.ToListAsync();

        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = "Consulta realizada",
            Countries = filter
        };
    }

}
