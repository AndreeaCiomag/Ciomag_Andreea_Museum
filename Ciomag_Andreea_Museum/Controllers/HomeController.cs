using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ciomag_Andreea_Museum.Models;
using Microsoft.EntityFrameworkCore;
using Ciomag_Andreea_Museum.Data;
using Ciomag_Andreea_Museum.Models.MuseumViewModels;

namespace Ciomag_Andreea_Museum.Controllers;

public class HomeController : Controller
{
    private readonly MuseumContext _context;

    public HomeController(MuseumContext context)
    {
        _context = context;
    }

    public async Task<ActionResult> Statistics()
    {
        IQueryable<VisitGroup> data = from visit in _context.Visits
                                      group visit by visit.VisitDate.Date into dateGroup
                                      select new VisitGroup()
                                      {
                                          VisitDate = dateGroup.Key,
                                          ClientsCount = dateGroup.Count()
                                      };
        return View(await data.AsNoTracking().ToListAsync());
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Chat()
    {
        return View();
    }
}

