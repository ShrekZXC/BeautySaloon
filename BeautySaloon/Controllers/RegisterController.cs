using System.Diagnostics;
using BeautySaloon.Middleware;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

[SiteNotAuthorize()]
public class RegisterController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserSerivce _userSerivce;

    public RegisterController(ILogger<LoginController> logger, IUserSerivce userSerivce)
    {
        _logger = logger;
        _userSerivce = userSerivce;
    }
    
    [HttpGet]
    [Route("register")]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }
        var user = new UserModel()
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            SecondName = model.SecondName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            Phone = model.Phone
        };

       await _userSerivce.Create(user);
        
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