using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class ServiceAppointmentsController : Controller
{
    private readonly ILogger<ServiceAppointmentsController> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceAppointmentService _serviceAppointmentService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly IServiceService _serviceService;

    public ServiceAppointmentsController(
        ILogger<ServiceAppointmentsController> logger, 
        IMapper mapper, 
        IServiceAppointmentService serviceAppointmentService, 
        UserManager<ApplicationUser> userManager,
        IUserService userService, 
        IServiceService serviceService)
    {
        _logger = logger;
        _mapper = mapper;
        _serviceAppointmentService = serviceAppointmentService;
        _userManager = userManager;
        _userService = userService;
        _serviceService = serviceService;
    }

    [HttpPost]
    public async Task<IActionResult> GetAllServiceAppointmentByWorkerId(Guid workerId)
    {
        var user = await _userManager.GetUserAsync(User);
        
        var serviceAppointmentsModel =
            await _serviceAppointmentService
                .GetServiceAppointmentsByWorkerIdAsync(workerId);

        var serviceAppointmentsViewModel =
            _mapper.Map<List<ServiceAppointmentsViewModel>>(serviceAppointmentsModel);

        var formattedAppointments = serviceAppointmentsViewModel.Select(serviceAppointment => new
        {
            Id = serviceAppointment.Id,
            Title = serviceAppointment.ClientId == user.Id ? $"Вы записаны на {serviceAppointment.Service.Name}" : "Занято",
            StartTime = $"{serviceAppointment.WorkDate:yyyy-MM-dd}T{serviceAppointment.StartTime}",
            EndTime = $"{serviceAppointment.WorkDate:yyyy-MM-dd}T{serviceAppointment.EndTime}",
        }).ToList();

        var test = DateTime.Now;

        return Json(formattedAppointments);
    }
    
[HttpPost]
public async Task<IActionResult> SaveServiceAppointment(
    Guid workerId,
    Guid serviceId, 
    string workDate, 
    string startTime)
{
    var user = await _userManager.GetUserAsync(User);
    
    if (workerId == Guid.Empty || string.IsNullOrEmpty(startTime))
    {
        return BadRequest("Invalid input");
    }

    try
    {
        var service = await _serviceService.Get(serviceId);
        var workDateTime = DateTime.Parse(workDate, null, System.Globalization.DateTimeStyles.RoundtripKind);
        var startTimeSpan = TimeSpan.Parse(startTime);
        var endTimeSpan = startTimeSpan.Add(TimeSpan.FromMinutes((double)service.Duration!));

        // Проверка на дату
        if (workDateTime.Date < DateTime.Today)
        {
            return BadRequest("Запись доступна минимум за день до.");
        }

        // Проверка на занятые слоты
        var existingAppointments =
            await _serviceAppointmentService.GetServiceAppointmentsByWorkerIdAsync(workerId);
        
        foreach (var appointment in existingAppointments)
        {
            if ((workDateTime.Date == appointment.WorkDate.Date) && (
                (startTimeSpan >= appointment.StartTime && startTimeSpan < appointment.EndTime) ||
                (endTimeSpan > appointment.StartTime && endTimeSpan <= appointment.EndTime) ||
                (startTimeSpan <= appointment.StartTime && endTimeSpan >= appointment.EndTime)))
            {
                return BadRequest("Выберите другую дату, этот временной слот уже занят.");
            }
        }

        var serviceAppointmentsModel = new ServiceAppointmentsModel()
        {
            WorkerId = workerId,
            ClientId = user.Id,
            ServiceId = serviceId,
            WorkDate = workDateTime,
            StartTime = startTimeSpan,
            EndTime = endTimeSpan
        };
        
        serviceAppointmentsModel = await _serviceAppointmentService.AddServiceAppointmentAsync(serviceAppointmentsModel);
        return Ok(new
        {
            id = serviceAppointmentsModel.Id,
            title = $"Вы записаны на {serviceAppointmentsModel.Service.Name}",
            start = serviceAppointmentsModel.WorkDate.ToString("yyyy-MM-dd") + "T" + serviceAppointmentsModel.StartTime,
            end = serviceAppointmentsModel.WorkDate.ToString("yyyy-MM-dd") + "T" + serviceAppointmentsModel.EndTime
        });
    }
    catch (System.Exception ex)
    {
        _logger.LogError(ex, "Error saving schedule");
        return StatusCode(500, "Internal server error");
    }
}
}