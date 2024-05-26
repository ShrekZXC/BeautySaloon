using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class AboutController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}