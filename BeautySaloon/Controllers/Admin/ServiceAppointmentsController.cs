using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers.Admin;

[Authorize(Roles = "Admin")]
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
        IMapper mapper, IServiceAppointmentService serviceAppointmentService, UserManager<ApplicationUser> userManager, IUserService userService, IServiceService serviceService)
    {
        _logger = logger;
        _mapper = mapper;
        _serviceAppointmentService = serviceAppointmentService;
        _userManager = userManager;
        _userService = userService;
        _serviceService = serviceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var workers = await _serviceAppointmentService.GetAllWorkers();
        var workersViewModel = _mapper.Map<List<WorkerViewModel>>(workers);
        
        return View("~/Views/Admin/ServiceAppointments/Index.cshtml", workersViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServiceAppointment()
    {
        var serviceAppointments = await _serviceAppointmentService.GetAllServiceAppointments();
        var serviceAppointmentsViewModel = _mapper.Map<List<ServiceAppointmentsViewModel>>(serviceAppointments);
        return View("~/Views/Admin/ServiceAppointments/ServiceAppointments.cshtml", serviceAppointmentsViewModel);
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
    public async Task<JsonResult> GetServiceAppointmentById(Guid serviceAppointmentId)
    {
        var serviceAppointmentsModel = await _serviceAppointmentService.GeServiceAppointmentById(serviceAppointmentId);
        var serviceAppointmentsViewModel = _mapper.Map<ServiceAppointmentsViewModel>(serviceAppointmentsModel);
        return Json(serviceAppointmentsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SetUpServiceAppointmentForWorker(Guid workerId)
    {
        var serviceAppointmentsModel = await _serviceAppointmentService.GetServiceAppointmentsByWorkerIdAsync(workerId);
        var worker = await _userManager.FindByIdAsync(workerId.ToString());
        ViewBag.WorkerName = $"{worker.FirstName} {worker.SecondName} {worker.LastName}";
        ViewBag.WorkerId = workerId; // Pass workerId to view
        var serviceAppointmentsViewModel = _mapper.Map<List<ServiceAppointmentsViewModel>>(serviceAppointmentsModel);
        return View("~/Views/Admin/ServiceAppointments/ServiceAppointment.cshtml", serviceAppointmentsViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveServiceAppointment(Guid workerId, Guid clientId, Guid serviceId, string workDate, string startTime)
    {
        if (workerId == Guid.Empty || string.IsNullOrEmpty(startTime))
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var service = await _serviceService.Get(serviceId);
            var serviceAppointmentsModel = new ServiceAppointmentsModel()
            {
                WorkerId = workerId,
                ClientId = clientId,
                ServiceId = serviceId,
                WorkDate = DateTime.Parse(workDate, null, System.Globalization.DateTimeStyles.RoundtripKind),
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(startTime).Add(TimeSpan.FromMinutes((double) service.Duration!))
            };
            
            serviceAppointmentsModel = await _serviceAppointmentService.AddServiceAppointmentAsync(serviceAppointmentsModel);
            return Ok(new
            {
                id = serviceAppointmentsModel.Id,
                title = $"Клиент: {serviceAppointmentsModel.Client.SecondName}" +
                        $" {serviceAppointmentsModel.Client.FirstName}" +
                        $" {serviceAppointmentsModel.Client.LastName}" +
                        $" Услуга: {serviceAppointmentsModel.Service.Name}",
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

    [HttpPost]
    public async Task<bool> DeleteServiceAppointmentById(Guid id)
    {
        var isDelete = await _serviceAppointmentService.DeleteServiceAppointmentById(id);
        return isDelete;
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateServiceAppointment(Guid eventId, Guid workerId, Guid clientId, Guid serviceId, string startTime)
    {
        try
        {
            var service = await _serviceService.Get(serviceId);
            var serviceAppointmentsModel = new ServiceAppointmentsModel()
            {
                Id = eventId,
                WorkerId = workerId,
                ClientId = clientId,
                ServiceId = serviceId,
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(startTime).Add(TimeSpan.FromMinutes((double) service.Duration!))
            };

            var serviceAppointmentsResult = await _serviceAppointmentService.UpdateServiceAppointmentAsync(serviceAppointmentsModel);
            return Ok(new
            {
                id = serviceAppointmentsResult.Id,
                title = $"Клиент: {serviceAppointmentsResult.Client.SecondName}" +
                        $" {serviceAppointmentsResult.Client.FirstName}" +
                        $" {serviceAppointmentsResult.Client.LastName}" +
                        $" Услуга: {serviceAppointmentsResult.Service.Name}",
                start = serviceAppointmentsResult.WorkDate.ToString("yyyy-MM-dd") + "T" + serviceAppointmentsResult.StartTime,
                end = serviceAppointmentsResult.WorkDate.ToString("yyyy-MM-dd") + "T" + serviceAppointmentsResult.EndTime
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error saving schedule");
            return StatusCode(500, "Internal server error");
        }
    }



}