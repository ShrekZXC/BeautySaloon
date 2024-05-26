using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class PromoController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> PromotionDetails(Guid id)
    {
        return Ok();
    }
}