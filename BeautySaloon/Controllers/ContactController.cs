using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class ContactController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}