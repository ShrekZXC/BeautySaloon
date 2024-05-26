using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Exception;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminUserController(
    ILogger<AdminBaseController> logger,
    ICurrentUser currentUser,
    IUserSerivce userService,
    IRoleService roleService,
    IMapper mapper)
    : AdminBaseController(logger, currentUser, userService, mapper)
{
    private readonly IRoleService _roleService = roleService;
    
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
    public async Task<IActionResult> Add()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }
        
        var userViewModel =new UserViewModel()
        {
            Id = Guid.NewGuid(),
            Roles = _mapper.Map<List<RoleViewModel>>(_roleService.GetAll())
        };

        return View("~/Views/Admin/user/add.cshtml", userViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(UserViewModel userViewModel)
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }
        
        try
        {
            var user = _mapper.Map<UserModel>(userViewModel);

            await _userService.Create(user);
        
            return RedirectToAction("Index");
        }
        catch (DuplicateEmailException)
        {
            ModelState.TryAddModelError("Email", "Email уже существует");
        }
        
        return View("~/Views/Admin/user/add.cshtml", userViewModel);
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