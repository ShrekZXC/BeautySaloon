using System.Diagnostics;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class RegisterController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public RegisterController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }
    
    [Route("register")]
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