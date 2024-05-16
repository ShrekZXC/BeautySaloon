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

    [Route("services")]
    public IActionResult Index()
    {
        var services = _serviceService.GetAll();
        var servicesViewModel = new ServicesViewModel();
        if (services.Count>0)
        {
            var serviceViewModel = _mapper.Map<List<ServiceViewModel>>(services);
            servicesViewModel.services = serviceViewModel;
        }
        
        return View(servicesViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}