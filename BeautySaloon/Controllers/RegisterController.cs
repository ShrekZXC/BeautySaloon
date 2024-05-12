using System.Diagnostics;
using AutoMapper;
using BeautySaloon.Exception;
using BeautySaloon.Middleware;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

[SiteNotAuthorize()]
public class RegisterController : Controller
{
    private readonly ILogger<RegisterController> _logger;
    private readonly IUserSerivce _userSerivce;
    private readonly IMapper _mapper;

    public RegisterController(ILogger<RegisterController> logger,
        IUserSerivce userSerivce,
        IMapper mapper)
    {
        _logger = logger;
        _userSerivce = userSerivce;
        _mapper = mapper;
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

        try
        {
            var user = _mapper.Map<UserModel>(model);

            await _userSerivce.Create(user);
        
            return RedirectToAction("RegistrationSuccess");
            //  return Redirect("/");
        }
        catch (DuplicateEmailException)
        {
            ModelState.TryAddModelError("Email", "Email уже существует");
        }
        
        return View("Index", model);
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