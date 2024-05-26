using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Model;
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
    
    [HttpGet]
    public async Task<IActionResult> UpdateUser(Guid id)
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var user = await _userService.Get(id);

        return View("~/Views/Admin/User/update.cshtml", _mapper.Map<UserViewModel>(user));
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateUser(UserViewModel userViewModel)
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var isUpdate = await _userService.Update(_mapper.Map<UserModel>(userViewModel));

        if (isUpdate)
        {
            return await Index();
        }
        else
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

    }
    
    [HttpPost]
    [Route("/admin/user/delete")]
    public async Task<IActionResult> Deleteuser()
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