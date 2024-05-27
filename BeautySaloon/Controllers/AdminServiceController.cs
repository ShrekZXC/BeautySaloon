using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminServiceController: Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<AdminServiceController> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceService _serviceService;

    public AdminServiceController(
        ICategoryService categoryService,
        ILogger<AdminServiceController> logger,
        IMapper mapper,
        IServiceService serviceService)
    {
        _categoryService = categoryService;
        _logger = logger;
        _mapper = mapper;
        _serviceService = serviceService;
    }

[HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = _mapper.Map<List<ServiceViewModel>>(_serviceService.GetAll());

        // Ваш код
        return View("~/Views/Admin/service/Index.cshtml", viewModel);
    }
    
        
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View("~/Views/Admin/service/add.cshtml", new ServiceViewModel
        {
            Id = Guid.NewGuid(),
            Categories = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetAll())
        });
    }
    
        
    [HttpPost]
    public async Task<IActionResult> Add(ServiceViewModel serviceViewModel, IFormFile Image)
    {
        if (Request.Form.Files.Count > 0)
        {
            WebFile webfile = new WebFile();
            string filename = webfile.GetWebFilename(Request.Form.Files[0].FileName);
            await webfile.UploadAndResizeImage(Request.Form.Files[0].OpenReadStream(), filename, 800, 600);
            serviceViewModel.ImageSrc = filename;
        }

        await _serviceService.Create(_mapper.Map<ServiceModel>(serviceViewModel));
        
        return await Index();
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var service = await _serviceService.Get(id);
        var serviceViewModel = _mapper.Map<ServiceViewModel>(service);
        serviceViewModel.Categories = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetAll());

        return View("~/Views/Admin/Service/update.cshtml", serviceViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(ServiceViewModel serviceViewModel, IFormFile ImageSrc, string CurrentImageSrc)
    {
        if (ImageSrc != null && ImageSrc.Length > 0)
        {
            WebFile webfile = new WebFile();
            string filename = webfile.GetWebFilename(Request.Form.Files[0].FileName);
            await webfile.UploadAndResizeImage(Request.Form.Files[0].OpenReadStream(), filename, 800, 600);
            serviceViewModel.ImageSrc = filename;
        }
        else
        {
            serviceViewModel.ImageSrc = CurrentImageSrc;
        }

        var isUpdate = await _serviceService.Update(_mapper.Map<ServiceModel>(serviceViewModel));

        if (isUpdate)
        {
            return await Index();
        }
        else
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        await _serviceService.Delete(id);

        return Json(new { success = true });
    }
    
}