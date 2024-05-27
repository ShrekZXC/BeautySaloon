using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

[Authorize(Roles = "Admin")]
public class AdminSchedule : Controller
{
    private readonly ILogger<AdminSchedule> _logger;
    private readonly IMapper _mapper;

    public AdminSchedule(
        ILogger<AdminSchedule> logger, 
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View("~/Views/Admin/schedule/Index.cshtml");
    }
}