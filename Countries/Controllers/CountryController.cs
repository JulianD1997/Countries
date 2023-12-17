using Countries.Models;
using Countries.Models.Dto;
using Countries.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Countries.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _countryService.GetAll();
        return response.IsSuccessful ? Ok(response) : NoContent();
    }
    [HttpGet("get-countries/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetCountries([FromQuery] int page = 1, int pageSize = 10)
    {
        var response = await _countryService.GetContries(page, pageSize);
        return Ok(response);
    }
    [HttpPost("save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Save(RegisterCountryDto country)
    {
        var response = await _countryService.Save(country);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    [HttpPost("add-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddHotels(AddObjectToCountry countryHotel) 
    {
        var response = await _countryService.AddHotels(countryHotel);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    [HttpPost("add-restaurants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRestaurants(AddObjectToCountry countryRestaurant)
    {
        var response = await _countryService.AddRestaurants(countryRestaurant);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateCountryDto updateCountry)
    {
        var response = await _countryService.Update(updateCountry);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
    [HttpPut("update-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCountries()
    {
        var response = await _countryService.UpdateList();
        return Ok(response);
    }
    [HttpDelete("delete/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string name)
    {
        var response = await _countryService.Delete(name);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
    
}
