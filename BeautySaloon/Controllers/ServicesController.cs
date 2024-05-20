using System.Diagnostics;
using AutoMapper;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class ServicesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServiceService _serviceService;
    private readonly IMapper _mapper;
    public ServicesController(ILogger<HomeController> logger,
        IServiceService serviceService,
        IMapper mapper)
    {
        _logger = logger;
        _serviceService = serviceService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("services")]
    public IActionResult Index()
    {
        var services = _serviceService.GetAll();
        
        var serviceViewModel = _mapper.Map<List<ServiceViewModel>>(services);
        
        return View(serviceViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}