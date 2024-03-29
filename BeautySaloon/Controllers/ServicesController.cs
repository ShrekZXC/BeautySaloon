using System.Diagnostics;
using BeautySaloon.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class ServicesController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ServicesController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("services")]
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}