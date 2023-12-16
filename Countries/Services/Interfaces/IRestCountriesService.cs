using Countries.Models;
using Countries.Models.Dto;

namespace Countries.Services.Interfaces;

public interface IRestCountriesService
{
    Task<List<Country>> GetAllCountries();
}
