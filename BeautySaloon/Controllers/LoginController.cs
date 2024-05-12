using System.Diagnostics;
using BeautySaloon.BL.Auth;
using BeautySaloon.Exception;
using BeautySaloon.Middleware;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

[SiteNotAuthorize()]
public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserSerivce _userSerivce;
    private readonly IAuth _auth;

    public LoginController(ILogger<LoginController> logger, 
        IUserSerivce userSerivce,
        IAuth auth)
    {
        _logger = logger;
        _userSerivce = userSerivce;
        _auth = auth;
    }
    
    [HttpGet]
    [Route("login")]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    [Route("login")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _auth.Authenticate(model.Email!, model.Password!, model.RememberMe == true);
                return Redirect("/");
            }
            catch (AuthorizationException)
            {
                ModelState.AddModelError("Email", "Email или пароль");
            }
        }

        return View("Index", model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}