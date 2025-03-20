using Microsoft.AspNetCore.Identity;

namespace EquiMarket.Models;

public class ApplicationUser : IdentityUser
{

    [PersonalData]
    public required string FirstName { get; set; }

    [PersonalData]
    public required string LastName { get; set; }
}