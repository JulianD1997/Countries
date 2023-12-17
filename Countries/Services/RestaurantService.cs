using AutoMapper;
using Countries.Data;
using Countries.Models;
using Countries.Models.Dto;
using Countries.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Countries.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IMapper _mapper;
    private readonly CountryDbContext _dbContext;

    public RestaurantService(CountryDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    //eliminar restaurante
    public async Task<Detail> Delete(int id)
    {
        var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        if (restaurant == null)
        {
            return new Detail
            {
                Status = ResponseStatus.NotFound,
                IsSuccessful = false,
                Message = "El restaurante que deseas eliminar no existe en la base de datos."
            };
        }
        _dbContext.Restaurants.Remove(restaurant);
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"{restaurant.Name} eliminado correctamente"
        };
    }
    //obtener todos los restaurantes
    public async Task<Detail> GetAll()
    {
        var listRestaurant = await _dbContext.Restaurants.ToListAsync();
        if (!listRestaurant.Any())
        {
            return new Detail
            {
                Status = ResponseStatus.NoContent,
                IsSuccessful = false,
                Message = "No hay dados por mostrar."
            };
        }
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = "Lista de restaurantes.",
            Restaurants= listRestaurant
        };
    }
    //guardar restaurante
    public async Task<Detail> Save(RegisterRestaurantDto restaurant)
    {
        var findRestaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Name.ToLower() == restaurant.Name.ToLower());
        if (findRestaurant == null)
        {
            var newRestaurant = _mapper.Map<Restaurant>(restaurant);
            await _dbContext.Restaurants.AddAsync(newRestaurant);
            await _dbContext.SaveChangesAsync();
            return new Detail
            {
                Status = ResponseStatus.Success,
                IsSuccessful = true,
                Message = $"{restaurant.Name} guardado correctamente."
            };
        }
        return new Detail
        {
            Status = ResponseStatus.BadRequest,
            IsSuccessful = false,
            Message = $"El restaurante {restaurant.Name} ya existe."
        };
    }
    //actualizar restaurante
    public async Task<Detail> Update(UpdateRestaurantDto updaterestaurant)
    {
        var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == updaterestaurant.Id);
        if (restaurant == null)
        {
            return new Detail
            {
                Status = ResponseStatus.NotFound,
                IsSuccessful = false,
                Message = $"El Restaurante {restaurant.Name} no existe en la base de datos."
            };
        }
        restaurant.Name = updaterestaurant.Name ?? restaurant.Name;
        restaurant.Type = updaterestaurant.Type ?? restaurant.Type;
        _dbContext.Restaurants.Update(restaurant);
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"El Restaurante {updaterestaurant.Name}, actualizado correctamente."
        };
    }
}
