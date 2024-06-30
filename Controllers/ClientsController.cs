namespace Tutorial9.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial9.Data;
using Tutorial9.Models;
using Tutorial9.Models.DTOs;
using Tutorial9.Services;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public ClientsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClient(int id, CancellationToken token)
    {
        var ClientToDelete = await _dbService.GetClient(id, token);
        var check = await _dbService.HasAnyTrips(id, token);
        if (check)
        {
            return StatusCode(403, "This client has trips assigned and cannot be deleted");
        }

        await _dbService.DeleteClient(ClientToDelete, token);
        return Ok();

    }
}