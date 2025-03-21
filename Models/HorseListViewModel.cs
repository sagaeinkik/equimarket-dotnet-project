namespace EquiMarket.Models;
public class HorseListViewModel
{
    public HorseSearchViewModel SearchModel { get; set; } = new HorseSearchViewModel();
    public List<HorseModel> Horses { get; set; } = new List<HorseModel>();
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
