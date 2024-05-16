using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Middleware;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminBaseController  : Controller
{
    private readonly ILogger<AdminBaseController > _logger;
    private readonly IUserSerivce _userService;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public AdminBaseController (ILogger<AdminBaseController > logger,
        ICurrentUser currentUser,
        IUserSerivce userService,
        IMapper mapper)
    {
        _logger = logger;
        _userService = userService;
        _currentUser = currentUser;
        _mapper = mapper;
    }
    protected async Task<IActionResult> CheckAdminAccess()
    {
        bool isLoggedIn = await _currentUser.IsLoggedIn();

        if (!isLoggedIn)
        {
            return Redirect("/login");
        }

        var userId = await _currentUser.GetCurrentUserId();
        if (userId != null)
        {
            var isAdmin = await _userService.IsAdmin((Guid)userId);
            if (!isAdmin)
            {
                return Redirect("/");
            }
        }

        return null;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}