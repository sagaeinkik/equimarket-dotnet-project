namespace EquiMarket.Models;

public class HorseSearchViewModel
{
    public string? Title { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public GenderType? Gender { get; set; }
    public string? Breed { get; set; }
    public AdType? AdType { get; set; }
    public string? Discipline { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
}