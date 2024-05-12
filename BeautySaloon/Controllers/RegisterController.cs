using System.Diagnostics;
using BeautySaloon.Model;
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
    
    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }
        var user = new User
        {
            FirstName = model.FirstName,
            SecondName = model.SecondName,
            Email = model.Email,
            Password = model.Password
        };
        
        return RedirectToAction("RegistrationSuccess");
    }

    public IActionResult RegistrationSuccess()
    {
        // Action for showing registration success page
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}