using System.Diagnostics;
using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPromotionService _promotionService;
    private readonly IServiceService _serviceService;
    private readonly ICategoryService _categoryService;
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger,
        IPromotionService promotionService,
        IServiceService serviceService,
        ICategoryService categoryService,
        IMapper mapper, IDbRepository dbRepository)
    {
        _logger = logger;
        _promotionService = promotionService;
        _serviceService = serviceService;
        _categoryService = categoryService;
        _mapper = mapper;
        _dbRepository = dbRepository;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel();
        homeViewModel.PromotionsViewModel = _mapper.Map<List<PromotionViewModel>>(_promotionService.GetAll());
        homeViewModel.CategoriesViewModel = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetAll());

        homeViewModel.MainSettings = await _dbRepository.Get<MainSettingsEntity>().FirstOrDefaultAsync();
        
        return View(homeViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}