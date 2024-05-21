using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.BL.General;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminCategoryController : AdminBaseController
{
    private readonly ICategoryService _categoryService;
    public AdminCategoryController(
        ILogger<AdminBaseController> logger,
        ICurrentUser currentUser,
        IUserSerivce userService,
        ICategoryService categoryService,
        IMapper mapper)
        : base(logger, currentUser, userService, mapper)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

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
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

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
}
