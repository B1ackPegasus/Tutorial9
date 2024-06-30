namespace Tutorial9.Models.DTOs;

public class TripListDTO
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public IEnumerable<TripDTO> Trips { get; set; } = new List<TripDTO>();
}