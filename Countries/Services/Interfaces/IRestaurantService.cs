using Countries.Models.Dto;
using Countries.Models;
using System.Threading.Tasks;
namespace Countries.Services.Interfaces;

public interface IRestaurantService
{
    Task<Detail> Save(RegisterRestaurantDto restaurant);
    Task<Detail> GetAll();
    Task<Detail> Update(UpdateRestaurantDto updaterestaurant);
    Task<Detail> Delete(int id);
}
