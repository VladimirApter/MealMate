using Domain.DataBaseAccess;
using Domain.Logic;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeoCoordinatesController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGeoCoordinates(long id)
    {
        var o = await DataBaseAccess<GeoCoordinates>.GetAsync(id);
        return o == null ? NotFound("GeoCoordinates is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostGeoCoordinates([FromBody] GeoCoordinates? geoCoordinates)
    {
        if (geoCoordinates == null) return NotFound("GeoCoordinates is null");
        
        DataBaseAccess<GeoCoordinates>.AddOrUpdate(geoCoordinates);
        return Ok(geoCoordinates.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteGeoCoordinates(long id)
    {
        var geoCoordinates = await DataBaseAccess<GeoCoordinates>.GetAsync(id);
        if (geoCoordinates == null) return NotFound("GeoCoordinates is null");

        DataBaseAccess<GeoCoordinates>.Delete(id);
        return Ok();
    }
}