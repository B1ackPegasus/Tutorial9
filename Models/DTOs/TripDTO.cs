using Azure.Core;

namespace Tutorial9.Models.DTOs;

public class TripDTO
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }
    
    public int MaxPeople { get; set; }

    public List<CountryDTO> Countries { get; set; } = new List<CountryDTO>();

    public List<ClientDTO> Clients { get; set; } = new List<ClientDTO>();
}