using System.Diagnostics;
using BeautySaloon.Middleware;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

[SiteNotAuthorize()]
public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }
    
    [Route("login")]
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