using AutoMapper;
using BeautySaloon.BL;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers.Admin;

[Authorize(Roles = "Admin")]
public class AdminCategoryController : Controller
{
    private readonly ILogger<AdminCategoryController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public AdminCategoryController(
        ILogger<AdminCategoryController> logger,
        ICategoryService categoryService,
        IMapper mapper)
    {
        _logger = logger;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var caregories = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetAll());

        return View("~/Views/Admin/Categories/Index.cshtml", caregories);
    }
    
    [HttpGet]
    public async Task<IActionResult> Update(Guid categoryId)
    {
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        return View("~/Views/Admin/Categories/add.cshtml", new CategoryViewModel()
        {
            Id = Guid.NewGuid()
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(CategoryViewModel categoryViewModel)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(CategoryViewModel categoryViewModel, IFormFile Image)
    {
        if (Request.Form.Files.Count > 0)
        {
            WebFile webfile = new WebFile();
            string filename = webfile.GetWebFilename(Request.Form.Files[0].FileName);
            await webfile.UploadAndResizeImage(Request.Form.Files[0].OpenReadStream(), filename, 800, 600);
            categoryViewModel.ImgSrc = filename;
        }

        await _categoryService.Create(_mapper.Map<CategoryModel>(categoryViewModel));

        return await Index();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.Delete(id);
        return Ok();
    }
}
