using System.ComponentModel.DataAnnotations;

namespace EquiMarket.Models;

public class HorseModel
{

    public int Id { get; set; }

    //Användare
    [Required(ErrorMessage = "Du måste vara inloggad för att skapa en annons.")]
    [Display(Name = "Användare")]
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    //Namn
    [Required(ErrorMessage = "Du måste fylla i hästens namn")]
    [StringLength(80, ErrorMessage = "Namnet måste vara mellan 2 och 80 tecken långt.", MinimumLength = 2)]
    [Display(Name = "Namn")]
    public string? Name { get; set; }

    //Födelsedatum
    [Required(ErrorMessage = "Du måste fylla i födelsedatum")]
    [DataType(DataType.Date)]
    [Display(Name = "Födelsedatum")]
    public DateTime BirthDate { get; set; } = DateTime.Now;

    //Kön
    [Required(ErrorMessage = "Du måste fylla i kön")]
    [Display(Name = "Kön")]
    public GenderType? Gender { get; set; }

    //Ras
    [Display(Name = "Ras")]
    public string? Breed { get; set; }

    //Mankhöjd
    [Display(Name = "Mankhöjd")]
    public int? Height { get; set; }

    //Annonstyp
    [Required(ErrorMessage = "Du måste fylla i annonstyp")]
    [Display(Name = "Annonstyp")]
    public AdType? AdType { get; set; }

    //Disciplin/inriktning
    [Display(Name = "Inriktning")]
    public string? Discipline { get; set; }

    //Undan
    [Display(Name = "u.")]
    public string? Dam { get; set; }

    //Efter
    [Display(Name = "e.")]
    public string? Sire { get; set; }

    //Undan efter
    [Display(Name = "ue.")]
    public string? DamSire { get; set; }

    //Efter efter
    [Display(Name = "ee.")]
    public string? GrandSire { get; set; }

    //Säljtitel
    [Required(ErrorMessage = "Du måste ange en annonstitel")]
    [StringLength(150, ErrorMessage = "Titeln måste vara mellan 2 och 150 tecken långt.", MinimumLength = 2)]
    [Display(Name = "Annonstitel")]
    public string? Title { get; set; }

    //Annonsinnehåll
    [Required(ErrorMessage = "Du måste fylla i annonsinnehåll")]
    [StringLength(5000, ErrorMessage = "Innehållet får vara max 5000 tecken långt.")]
    [Display(Name = "Annonsinnehåll")]
    public string? Content { get; set; }

    //Bild
    [Display(Name = "Bild")]
    public string? ImagePath { get; set; }

    //Pris
    [Display(Name = "Pris")]
    [Required(ErrorMessage = "Du måste fylla i pris.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pris måste vara minst 1kr.")]
    public int Price { get; set; }


    public DateTime? CreatedDate { get; set; }
}