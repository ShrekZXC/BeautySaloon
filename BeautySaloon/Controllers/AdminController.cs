using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminController : AdminBaseController
{
    public AdminController(ILogger<AdminBaseController> logger, ICurrentUser currentUser, IUserSerivce userService, IMapper mapper) : base(logger, currentUser, userService, mapper)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        // Ваш код
        return View();
    }
}