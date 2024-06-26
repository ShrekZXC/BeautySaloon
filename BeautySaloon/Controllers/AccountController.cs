﻿using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AccountController : Controller
{
    private readonly string? UserRole = "User";
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IServiceAppointmentService _serviceAppointmentService;

    public AccountController(
        IMapper mapper,
        IUserService userService, UserManager<ApplicationUser> userManager, IServiceAppointmentService serviceAppointmentService)
    {
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
        _serviceAppointmentService = serviceAppointmentService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<UserModel>(loginViewModel);
            var result = await _userService.Login(user, loginViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверные данные для входа");
        }

        return View(loginViewModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<UserModel>(registerViewModel);

            var result = await _userService.RegisterUserAsync(user, registerViewModel.Password, UserRole);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(registerViewModel);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _userService.Logout();
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult IsAuthenticated()
    {
        bool isAuthenticated = User.Identity is {IsAuthenticated: true};
        return Json(new {isAuthenticated});
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var appUser = await _userManager.GetUserAsync(User);
        var user = await _userService.FindByIdAsync(appUser.Id);

        var profile = _mapper.Map<ProfileViewModel>(user);
        var appointments = await _serviceAppointmentService.GetAllServiceAppointmentsByClientId(appUser.Id);

        profile.Appointments = _mapper.Map<List<ServiceAppointmentsViewModel>>(appointments);

        return View(profile);
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}