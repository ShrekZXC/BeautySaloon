﻿using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.BL.General;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminServiceController(
    ICategoryService categoryService,
    ILogger<AdminServiceController> logger,
    ICurrentUser currentUser,
    IUserSerivce userService,
    IMapper mapper,
    IServiceService serviceService)
    : AdminBaseController(logger, currentUser, userService, mapper)
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IServiceService _serviceService = serviceService ?? throw new ArgumentNullException(nameof(serviceService));

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var viewModel = _mapper.Map<List<ServiceViewModel>>(_serviceService.GetAll());

        // Ваш код
        return View("~/Views/Admin/service/Index.cshtml", viewModel);
    }
    
        
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

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
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var service = await _serviceService.Get(id);
        var serviceViewModel = _mapper.Map<ServiceViewModel>(service);
        serviceViewModel.Categories = _mapper.Map<List<CategoryViewModel>>(_categoryService.GetAll());

        return View("~/Views/Admin/Service/update.cshtml", serviceViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(ServiceViewModel serviceViewModel, IFormFile ImageSrc, string CurrentImageSrc)
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }
        
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
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        await _serviceService.Delete(id);

        return Json(new { success = true });
    }
    
}