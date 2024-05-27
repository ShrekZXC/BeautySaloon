using System.Diagnostics;
using AutoMapper;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPromotionService _promotionService;
    private readonly IServiceService _serviceService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger,
        IPromotionService promotionService,
        IServiceService serviceService,
        ICategoryService categoryService,
        IMapper mapper)
    {
        _logger = logger;
        _promotionService = promotionService;
        _serviceService = serviceService;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel();
        homeViewModel.PromotionsViewModel = _mapper.Map<List<PromotionViewModel>>(_promotionService.GetAll());
        homeViewModel.CategoriesViewModel = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetAll());
        
        
        return View(homeViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}