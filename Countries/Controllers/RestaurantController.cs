using Countries.Models.Dto;
using Countries.Models;
using Countries.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Countries.Services;

namespace Countries.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _RestaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _RestaurantService = restaurantService;
    }

    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _RestaurantService.GetAll();
        return response.IsSuccessful ? Ok(response) : NoContent();
    }
    [HttpPost("save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Save(RegisterRestaurantDto restaurant)
    {
        var response = await _RestaurantService.Save(restaurant);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateRestaurantDto updateRestaurant)
    {
        var response = await _RestaurantService.Update(updateRestaurant);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _RestaurantService.Delete(id);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
}
