using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class LicenseController: Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}