using AutoMapper;
using BeautySaloon.DAL.Entity;
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
        var serviceAppointmentsModel =
            await _serviceAppointmentService
                .GetServiceAppointmentsByWorkerIdAsync(workerId);

        var serviceAppointmentsViewModel =
            _mapper.Map<List<ServiceAppointmentsViewModel>>(serviceAppointmentsModel);

        var formattedAppointments = serviceAppointmentsViewModel.Select(serviceAppointment => new
        {
            Id = serviceAppointment.Id,
            StartTime = $"{serviceAppointment.WorkDate:yyyy-MM-dd}T{serviceAppointment.StartTime}",
            EndTime = $"{serviceAppointment.WorkDate:yyyy-MM-dd}T{serviceAppointment.EndTime}",
        }).ToList();

        return Json(formattedAppointments);
    }
}