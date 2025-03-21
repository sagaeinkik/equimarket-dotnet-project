using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EquiMarket.Models;
using EquiMarket.Data;

namespace EquiMarket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        //Välj ut 3 random annonser
        var randomAds = _context.Horses.AsEnumerable()
            .OrderBy(r => Guid.NewGuid()) //Blanda runt
            .Take(3) //Kapa vid 3
            .ToList();

        return View(randomAds);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
