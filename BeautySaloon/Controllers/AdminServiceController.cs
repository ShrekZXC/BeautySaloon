using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminServiceController(
    ILogger<AdminBaseController> logger,
    ICurrentUser currentUser,
    IUserSerivce userService,
    IMapper mapper)
    : AdminBaseController(logger, currentUser, userService, mapper)
{
    [HttpGet]
    [Route("/admin/service")]
    public async Task<IActionResult> Index()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        // Ваш код
        return View("~/Views/Admin/User/Index.cshtml");
    }
}