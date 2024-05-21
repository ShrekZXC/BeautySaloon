using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminPromotion : AdminBaseController
{
    public AdminPromotion(ILogger<AdminBaseController> logger,
        ICurrentUser currentUser,
        IUserSerivce userService,
        IMapper mapper) : 
        base(logger, currentUser, userService, mapper)
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

        return View("~/Views/Admin/promotion/Index.cshtml");
    }
}