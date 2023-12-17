using Countries.Models.Dto;
using Countries.Models;

namespace Countries.Services.Interfaces;

public interface IHotelService
{
    Task<Detail> Save(RegisterHotelDto hotel);
    Task<Detail> GetAll();
    Task<Detail> Update(UpdateHotelDto updateHotel);
    Task<Detail> Delete(int id);
}
