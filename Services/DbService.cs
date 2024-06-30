using Microsoft.EntityFrameworkCore;
using Tutorial9.Data;
using Tutorial9.Models;
using Tutorial9.Models.DTOs;

namespace Tutorial9.Services;

public class DbService : IDbService
{
    private readonly MasterContext _context;

    public DbService(MasterContext context)
    {
        _context = context;
    }

    public async Task<List<TripDTO>> GetTrips(CancellationToken token)
    {
        return await _context.Trips.Select(trip => new TripDTO()
        {
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            Countries = _context.Countries.Where(country => country.IdTrips.Any(id => id.IdTrip == trip.IdTrip)).Select(country => new CountryDTO()
            {
                Name = country.Name
            }).ToList(),
            Clients = _context.Clients.Where(c => c.ClientTrips.Any(ct => ct.IdTrip == trip.IdTrip)).Select(client => new ClientDTO()
            {
                FirstName = client.FirstName,
                LastName = client.LastName
            }).ToList()
        }).OrderByDescending(e => e.DateFrom).ToListAsync(token);
    }

    public async Task<Client?> GetClient(int id, CancellationToken token)
    {
        return await _context.Clients.Where(c => c.IdClient == id).FirstOrDefaultAsync(token);
    }

    public async Task DeleteClient(Client ClientToDelete, CancellationToken token)
    {
        _context.Clients.Remove(ClientToDelete);
        await _context.SaveChangesAsync(token);
    }

    public async Task<bool> GetClientTrips(ClientTripDTO clientTrip, CancellationToken token)
    {
        var client = await GetClientByPesel(clientTrip.Pesel, token);
        if (client == null)
        {
            return false;
        }
        
        var resClientTrip = await _context.ClientTrips.Where(cl => cl.IdTrip == clientTrip.IdTrip && cl.IdClient == client.IdClient)
            .FirstOrDefaultAsync(token);
        if (resClientTrip != null)
        {
            return false;
        }

        return true;

    }

    public async Task<Client?> GetClientByPesel(string pesel, CancellationToken token)
    {
       return await _context.Clients.Where(cl => cl.Pesel == pesel).FirstOrDefaultAsync(token);
    }

    public async Task<bool> GetTrip(int id, CancellationToken token)
    {
        var trip = await _context.Trips.Where(t => t.IdTrip == id).FirstOrDefaultAsync(token);
        if (trip == null)
        {
            return false;
        }

        if (trip.DateFrom.CompareTo(DateTime.Now) < 0)
        {
            return false;
        }

        return true;
    }

    public async Task AddClientToTrip(ClientTripDTO clientTripDto, CancellationToken token)
    {
        var client = await GetClientByPesel(clientTripDto.Pesel, token);
        await _context.ClientTrips.AddAsync(new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = clientTripDto.IdTrip,
            PaymentDate = DateTime.Parse(clientTripDto.PaymentDate),
            RegisteredAt = DateTime.Now
        }, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task<bool> HasAnyTrips(int id, CancellationToken token)
    {
        var trips = await _context.ClientTrips.Where(ct => ct.IdClient == id).ToListAsync(token);
        return trips.Count != 0;
    }
}