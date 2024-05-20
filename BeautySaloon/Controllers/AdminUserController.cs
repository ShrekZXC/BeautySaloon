using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminUserController(
    ILogger<AdminBaseController> logger,
    ICurrentUser currentUser,
    IUserSerivce userService,
    IMapper mapper)
    : AdminBaseController(logger, currentUser, userService, mapper)
{
    [HttpGet]
    [Route("/admin/user")]
    public async Task<IActionResult> Index()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var users = _userService.GetAll();

        return View("~/Views/Admin/User/Index.cshtml", _mapper.Map<List<UserViewModel>>(users));
    }
}