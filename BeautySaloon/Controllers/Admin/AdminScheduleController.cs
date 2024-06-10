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
    private readonly IUserService _userService;
    private readonly IServiceService _serviceService;

    public AdminScheduleController(
        ILogger<AdminScheduleController> logger, 
        IMapper mapper, IScheduleService scheduleService, UserManager<ApplicationUser> userManager, IUserService userService, IServiceService serviceService)
    {
        _logger = logger;
        _mapper = mapper;
        _scheduleService = scheduleService;
        _userManager = userManager;
        _userService = userService;
        _serviceService = serviceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var workers = await _scheduleService.GetAllWorkers();
        var workersViewModel = _mapper.Map<List<WorkerViewModel>>(workers);
        
        return View("~/Views/Admin/schedule/Index.cshtml", workersViewModel);
    }

    [HttpGet]
    public async Task<JsonResult> GetAllClients()
    {
        var clientsModel = await _userService.GetAllClients();
        var clientsViewModel = _mapper.Map<List<ClientViewModel>>(clientsModel);
        return Json(clientsViewModel);
    }

    [HttpGet]
    public Task<JsonResult> GetAllService()
    {
        var servicesModel =  _serviceService.GetAll();
        var servicesViewModel = _mapper.Map<List<ServiceModel>, List<ServiceViewModel>>(servicesModel);
        return Task.FromResult(Json(servicesViewModel));
    }

    [HttpGet]
    public async Task<JsonResult> GetScheduleById(Guid scheduleId)
    {
        var scheduleModel = await _scheduleService.GetWorkScheduleById(scheduleId);
        var scheduleViewModel = _mapper.Map<WorkScheduleViewModel>(scheduleModel);
        return Json(scheduleViewModel);
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
    public async Task<IActionResult> SaveSchedule(Guid workerId, Guid clientId, Guid serviceId, string workDate, string startTime)
    {
        if (workerId == Guid.Empty || string.IsNullOrEmpty(startTime))
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var service = await _serviceService.Get(serviceId);
            var schedule = new WorkScheduleModel()
            {
                WorkerId = workerId,
                ClientId = clientId,
                ServiceId = serviceId,
                WorkDate = DateTime.Parse(workDate, null, System.Globalization.DateTimeStyles.RoundtripKind),
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(startTime).Add(TimeSpan.FromMinutes((double) service.Duration!))
            };
            
            schedule = await _scheduleService.AddWorkScheduleAsync(schedule);
            return Ok(new
            {
                id = schedule.Id,
                title = $"Клиент: {schedule.Client.SecondName} {schedule.Client.FirstName} {schedule.Client.LastName} Услуга: {schedule.Service.Name}",
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
    public async Task<bool> DeleteById(Guid id)
    {
        var isDelete = await _scheduleService.DeleteById(id);
        return isDelete;
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateSchedule(Guid eventId, Guid workerId, Guid clientId, Guid serviceId, string startTime)
    {
        try
        {
            var service = await _serviceService.Get(serviceId);
            var schedule = new WorkScheduleModel()
            {
                Id = eventId,
                WorkerId = workerId,
                ClientId = clientId,
                ServiceId = serviceId,
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(startTime).Add(TimeSpan.FromMinutes((double) service.Duration!))
            };

            var scheduleResult = await _scheduleService.UpdateWorkScheduleAsync(schedule);
            return Ok(new
            {
                id = scheduleResult.Id,
                title = $"Клиент: {scheduleResult.Client.SecondName} {scheduleResult.Client.FirstName} {scheduleResult.Client.LastName} Услуга: {scheduleResult.Service.Name}",
                start = scheduleResult.WorkDate.ToString("yyyy-MM-dd") + "T" + scheduleResult.StartTime,
                end = scheduleResult.WorkDate.ToString("yyyy-MM-dd") + "T" + scheduleResult.EndTime
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error saving schedule");
            return StatusCode(500, "Internal server error");
        }
    }



}