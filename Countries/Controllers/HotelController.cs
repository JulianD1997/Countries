using Countries.Models.Dto;
using Countries.Models;
using Countries.Services;
using Countries.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Countries.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }
    /// <summary>
    /// lista todos los hoteles.
    /// </summary>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="204">no hay elementos por mostrar</response>
    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _hotelService.GetAll();
        return response.IsSuccessful ? Ok(response) : NoContent();
    }
    /// <summary>
    /// guarda un hotel.
    /// </summary>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="400">hubo un problema al guardar el hotel</response>
    [HttpPost("save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Save(RegisterHotelDto hotel)
    {
        var response = await _hotelService.Save(hotel);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    /// <summary>
    /// actualiza un hotel por medio de su id.
    /// </summary>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="404">no se encontro el hotel que se desea actualizar</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateHotelDto updateHotel)
    {
        var response = await _hotelService.Update(updateHotel);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
    /// <summary>
    /// elimina un hotel por medio de su id.
    /// </summary>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="404">no se encontro el hotel que se desea eliminar</response>
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _hotelService.Delete(id);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
}
