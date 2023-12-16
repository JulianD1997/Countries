using AutoMapper;
using Countries.Data;
using Countries.Models;
using Countries.Models.Dto;
using Countries.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.Metrics;
namespace Countries.Services;

public class HotelService : IHotelService
{
    private readonly IMapper _mapper;
    private readonly CountryDbContext _dbContext;

    public HotelService(CountryDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Detail> Delete(int id)
    {
        var hotel = await _dbContext.Hotels.FirstOrDefaultAsync(h => h.Id == id);
        if (hotel == null)
        {
            return new Detail
            {
                Status = ResponseStatus.NotFound,
                IsSuccessful = false,
                Message = "El hotel que deseas eliminar no existe en la base de datos."
            };
        }
        _dbContext.Hotels.Remove(hotel);
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"{hotel.Name} eliminado correctamente"
        };
    }

    public async Task<Detail> GetAll()
    {
        var listHotel = await _dbContext.Hotels.ToListAsync();
        if (!listHotel.Any())
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
            Message = "Lista de hoteles.",
            Hotels = listHotel
        };
    }

    public async Task<Detail> Save(RegisterHotelDto hotel)
    {
        var findHotel = await _dbContext.Hotels.FirstOrDefaultAsync(h => h.Name.ToLower() == hotel.Name.ToLower());
        if (findHotel == null)
        {
            var newHotel = _mapper.Map<Hotel>(hotel);
            await _dbContext.Hotels.AddAsync(newHotel);
            await _dbContext.SaveChangesAsync();
            return new Detail
            {
                Status = ResponseStatus.Success,
                IsSuccessful = true,
                Message = $"{hotel.Name} guardado correctamente."
            };
        }
        return new Detail
        {
            Status = ResponseStatus.BadRequest,
            IsSuccessful = false,
            Message = $"El hotel {hotel.Name} ya existe."
        };
    }

    public async Task<Detail> Update(UpdateHotelDto updateHotel)
    {
        var hotel = await _dbContext.Hotels.FirstOrDefaultAsync(h => h.Id == updateHotel.Id);
        if (hotel == null)
        {
            return new Detail
            {
                Status = ResponseStatus.NotFound,
                IsSuccessful = false,
                Message = $"El hotel {updateHotel.Name} no existe en la base de datos."
            };
        }
        hotel.Name = updateHotel.Name ?? hotel.Name;
        hotel.Starts = updateHotel.Starts ?? hotel.Starts;
        _dbContext.Hotels.Update(hotel);
        await _dbContext.SaveChangesAsync();
        return new Detail
        {
            Status = ResponseStatus.Success,
            IsSuccessful = true,
            Message = $"El Hotel {updateHotel.Name}, actualizado correctamente."
        };
    }
}
