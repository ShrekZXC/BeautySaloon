using System.Diagnostics;
using AutoMapper;
using BeautySaloon.BL;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AdminPromotion : Controller
{
    private readonly ILogger<AdminPromotion> _logger;
    private readonly IPromotionService _promotionService;
    private readonly IMapper _mapper;

    public AdminPromotion(ILogger<AdminPromotion> logger,
        IPromotionService promotionService,
        IMapper mapper)
    {
        _logger = logger;
        _promotionService = promotionService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var promos = _promotionService.GetAll();

        var promoViewModel = _mapper.Map<List<PromotionViewModel>>(promos);

        return View("~/Views/Admin/promotion/Index.cshtml", promoViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var promotionViewModel = new PromotionViewModel()
        {
            Id = Guid.NewGuid()
        };

        return View("~/Views/Admin/promotion/add.cshtml", promotionViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(PromotionViewModel promotionViewModel)
    {
        var promo = _mapper.Map<PromotionModel>(promotionViewModel);

        await _promotionService.Create(promo);
        
        return await Index();
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var promo = await _promotionService.Get(id);

        return View("~/Views/Admin/promotion/update.cshtml", _mapper.Map<PromotionViewModel>(promo));
    }

    [HttpPost]
    public async Task<IActionResult> Update(PromotionViewModel promotionViewModel, IFormFile ImgSrc, string CurrentImageSrc)
    {
        if (ImgSrc != null && ImgSrc.Length > 0)
        {
            WebFile webfile = new WebFile();
            string filename = webfile.GetWebFilename(Request.Form.Files[0].FileName);
            await webfile.UploadAndResizeImage(Request.Form.Files[0].OpenReadStream(), filename, 800, 600);
            promotionViewModel.ImgSrc = filename;
        }
        else
        {
            promotionViewModel.ImgSrc = CurrentImageSrc;
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
        await _promotionService.Delete(id);

        return Json(new {success = true});
    }
}