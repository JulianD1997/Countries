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
    /// <summary>
    /// Obtiene todos los paises con sus respectivos hoteles y restaurantes.
    /// </summary>
    /// <returns>Un detail con informacion detallada y la lista</returns>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="204">No se encontraron paises.</response>
    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _countryService.GetAll();
        return response.IsSuccessful ? Ok(response) : NoContent();
    }
    /// <summary>
    /// Obtiene países utilizando paginación.
    /// </summary>
    /// <param name="page"> Numero de pagina</param>
    /// <param name="pageSize"> Numero de tamaño de objetos por pagina</param>
    /// <returns>Un detail con informacion detallada y la lista de paises por paginacion</returns>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="204">No se encontraron paises.</response>
    [HttpGet("get-countries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetCountries([FromQuery] int page = 1, int pageSize = 10)
    {
        var response = await _countryService.GetContries(page, pageSize);
        return Ok(response);
    }
    /// <summary>
    /// Obtiene países utilizando filtrado.
    /// </summary>
    /// <param name="country"> nombre del pais que se quiere buscar.</param>
    /// <param name="iso"> codigo ISO del pais que se quiere buscar.</param>
    /// <param name="restaurant"> nombre del restaurante que se quiere buscar.</param>
    /// <param name="type"> Tipo de restaurante que se quiere buscar.</param>
    /// <param name="hotel"> Nombre del hotel que se quiere buscar.</param>
    /// <param name="starts"> Numero de estrellas que se quiere buscar.</param>  
    /// <returns>Un detail con informacion detallada y la lista de paises por filtros especificados</returns>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="204">No se encontraron paises.</response>
    [HttpGet("countries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> FilterObject(
        [FromQuery] string country = null,
        [FromQuery] string iso = null,
        [FromQuery] string restaurant = null,
        [FromQuery] string type = null,
        [FromQuery] string hotel = null,
        [FromQuery] string starts = null
        )
    {
        var response = await _countryService.filterObject(country, iso, restaurant, type,hotel, starts);
        return response.IsSuccessful ? Ok(response) : NoContent();
    }
    /// <summary>
    /// Guarda un país en el sitema.
    /// </summary>
    /// <param name="country">Datos del país nuevo.</param>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="400">Hubo un error o problema al guardar el pais.</response>
    [HttpPost("save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Save(RegisterCountryDto country)
    {
        var response = await _countryService.Save(country);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    /// <summary>
    /// Guarda hoteles por medio de un listado de Ids y el nombre de pais.
    /// </summary>
    /// <param name="countryHotel">nombre del pais y listado de Ids de los hoteles que se desean relacionar.</param>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="400">Hubo un error o problema al realizar esta consulta.</response>
    [HttpPost("add-hotels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddHotels(AddObjectToCountry countryHotel) 
    {
        var response = await _countryService.AddHotels(countryHotel);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    /// <summary>
    /// Guarda restaurantes por medio de un listado de Ids y el nombre de pais.
    /// </summary>
    /// <param name="countryRestaurant">nombre del pais y listado de Ids de los restaurantes que se desean relacionar.</param>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="400">Hubo un error o problema al realizar esta consulta.</response>
    [HttpPost("add-restaurants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddRestaurants(AddObjectToCountry countryRestaurant)
    {
        var response = await _countryService.AddRestaurants(countryRestaurant);
        return response.IsSuccessful ? Ok(response) : BadRequest(response);
    }
    /// <summary>
    /// Actualiza datos del pais.
    /// </summary>
    /// <param name="updateCountry">id del pais y los nuevos datos actualizados</param>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="404">no se encontro el pais que se desea actualizar</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateCountryDto updateCountry)
    {
        var response = await _countryService.Update(updateCountry);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
    /// <summary>
    /// Agrega paises por medio de un API de paises.
    /// </summary>
    /// <response code="200">Respuesta sastifactoria.</response>
    [HttpPut("update-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCountries()
    {
        var response = await _countryService.UpdateList();
        return Ok(response);
    }
    /// <summary>
    /// elimina un pais por medio de su nombre.
    /// </summary>
    /// <response code="200">Respuesta sastifactoria.</response>
    /// <response code="404">no se encontro el pais que se desea eliminar</response>
    [HttpDelete("delete/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Detail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string name)
    {
        var response = await _countryService.Delete(name);
        return response.Status == ResponseStatus.Success ? Ok(response) : NotFound(response);
    }
}
