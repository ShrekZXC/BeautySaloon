using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers.Admin;

public class AdminUiController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View("~/Views/Admin/UI/Index.cshtml");
    }
}