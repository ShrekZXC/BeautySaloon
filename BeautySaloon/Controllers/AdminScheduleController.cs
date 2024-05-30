using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautySaloon.Controllers;

[Authorize(Roles = "Admin")]
public class AdminScheduleController : Controller
{
    private readonly ILogger<AdminScheduleController> _logger;
    private readonly IMapper _mapper;
    private readonly IScheduleService _scheduleService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminScheduleController(
        ILogger<AdminScheduleController> logger, 
        IMapper mapper, IScheduleService scheduleService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _scheduleService = scheduleService;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var workers = await _scheduleService.GetAllWorkers();
        var workersViewModel = _mapper.Map<List<WorkerViewModel>>(workers);
        
        return View("~/Views/Admin/schedule/Index.cshtml", workersViewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.GetUsersInRoleAsync("User");
        var userList = users.Select(user => new {
            Id = user.Id,
            FullName = user.FirstName + " " + user.LastName
        }).ToList();

        return Ok(userList);
    }


    [HttpPost]
    public async Task<IActionResult> SetUpSchedule(Guid workerId)
    {
        var schedules = await _scheduleService.GetWorkSchedulesAsync(workerId);
        var worker = await _userManager.FindByIdAsync(workerId.ToString());
        ViewBag.WorkerName = $"{worker.FirstName} {worker.SecondName} {worker.LastName}";
        ViewBag.WorkerId = workerId; // Pass workerId to view
        var scheduleViewModel = _mapper.Map<List<WorkScheduleViewModel>>(schedules);
        return View("~/Views/Admin/schedule/schedule.cshtml", scheduleViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveSchedule(Guid workerId, DateTime workDate, string startTime, string endTime)
    {
        if (workerId == Guid.Empty || string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var schedule = new WorkScheduleModel()
            {
                WorkerId = workerId,
                WorkDate = workDate,
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(endTime)
            };

            schedule.Id = await _scheduleService.AddWorkScheduleAsync(schedule);
            schedule.Worker =  _mapper.Map<WorkerModel>(await _userManager.FindByIdAsync(workerId.ToString()));
            return Ok(new
            {
                id = schedule.Id,
                title = $"{schedule.Worker.FirstName} {schedule.Worker.LastName}",
                start = schedule.WorkDate.ToString("yyyy-MM-dd") + "T" + schedule.StartTime,
                end = schedule.WorkDate.ToString("yyyy-MM-dd") + "T" + schedule.EndTime
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error saving schedule");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateSchedule(Guid workerId, Guid workScheduleId, string startTime, string endTime)
    {
        if (workerId == Guid.Empty || workScheduleId == Guid.Empty || string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var schedule = new WorkScheduleModel()
            {
                Id = workScheduleId,
                WorkerId = workerId,
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(endTime)
            };

            await _scheduleService.UpdateWorkScheduleAsync(schedule);
            return Ok();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error saving schedule");
            return StatusCode(500, "Internal server error");
        }
    }



}