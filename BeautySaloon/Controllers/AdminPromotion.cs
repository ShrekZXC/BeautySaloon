using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminPromotion : AdminBaseController
{
    private readonly IPromotionService _promotionService;
    public AdminPromotion(ILogger<AdminBaseController> logger,
        ICurrentUser currentUser,
        IUserSerivce userService,
        IPromotionService promotionService,
        IMapper mapper) :
        base(logger, currentUser, userService, mapper)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var promos = _promotionService.GetAll();

        var promoViewModel = _mapper.Map<List<PromotionViewModel>>(promos);

        return View("~/Views/Admin/promotion/Index.cshtml", promoViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var promotionViewModel = new PromotionViewModel()
        {
            Id = Guid.NewGuid()
        };

        return View("~/Views/Admin/promotion/add.cshtml", promotionViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(PromotionViewModel promotionViewModel)
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }


        var promo = _mapper.Map<PromotionModel>(promotionViewModel);

        await _promotionService.Create(promo);
        
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

        var promo = await _promotionService.Get(id);

        return View("~/Views/Admin/promotion/update.cshtml", _mapper.Map<PromotionViewModel>(promo));
    }

    [HttpPost]
    public async Task<IActionResult> Update(PromotionViewModel promotionViewModel)
    {
        var accessResult = await CheckAdminAccess();
        if (accessResult != null)
        {
            return accessResult;
        }

        var isUpdate = await _promotionService.Update(_mapper.Map<PromotionModel>(promotionViewModel));

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

        await _promotionService.Delete(id);

        return Json(new {success = true});
    }
}