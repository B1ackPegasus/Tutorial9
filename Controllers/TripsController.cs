using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial9.Data;
using Tutorial9.Models;
using Tutorial9.Models.DTOs;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly IDbService _dbService;
    
    public TripsController(IDbService dbService )
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(int page, int pageSize, CancellationToken token)
    {
        var trips = await _dbService.GetTrips(token);

        if (page == 0)
        {
            page = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 10;
        }

        var result = trips.Skip((page - 1) * pageSize).Take(pageSize);

        var allPages = trips.Count / pageSize;
        var modulo = trips.Count % pageSize;
        if (modulo != 0)
        {
            allPages += 1;
        }

        return Ok(new TripListDTO()
        {
            Page = page,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = result
        });
    }

    [HttpPost("{id:int}/clients")]
    public async Task<IActionResult> AssignClientToTrip(ClientTripDTO clientTrip, CancellationToken token)
    {
        var check = await _dbService.GetClientByPesel(clientTrip.Pesel, token);
        if (check != null)
        {
            return StatusCode(403, "This client already exists");
        }
        
        var checkBool = await _dbService.GetClientTrips(clientTrip, token);
        if (!checkBool)
        {
            return StatusCode(403, "This client already assigned to this trip");
        }

        var checkTrip = await _dbService.GetTrip(clientTrip.IdTrip, token);

        if (!checkTrip)
        {
            return StatusCode(403, "You cannot assign client to this trip");
        }

        await _dbService.AddClientToTrip(clientTrip, token);
        return Created();
    }
    
}