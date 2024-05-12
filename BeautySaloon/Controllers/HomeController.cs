using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeautySaloon.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserSerivce _userSerivce;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger,
        ICurrentUser currentUser,
        IUserSerivce userSerivce,
        IMapper mapper)
    {
        _logger = logger;
        _userSerivce = userSerivce;
        _currentUser = currentUser;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}