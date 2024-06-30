using Azure.Core;
using Tutorial9.Models;
using Tutorial9.Models.DTOs;

namespace Tutorial9.Services;

public interface IDbService
{
    public Task<List<TripDTO>> GetTrips(CancellationToken token);
    public Task<Client?> GetClient(int id, CancellationToken token);
    public Task DeleteClient(Client ClientToDelete, CancellationToken token);
    public Task<bool> GetClientTrips(ClientTripDTO clientTrip, CancellationToken token);
    public Task<Client?> GetClientByPesel(string pesel, CancellationToken token);
    public Task<bool> GetTrip(int id, CancellationToken token);
    public Task AddClientToTrip(ClientTripDTO clientTripDto, CancellationToken token);
    public Task<bool> HasAnyTrips(int id, CancellationToken token);
}