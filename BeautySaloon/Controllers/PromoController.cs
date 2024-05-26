using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class PromoController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}