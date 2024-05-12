using BeautySaloon.BL.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly ICurrentUser _currentUser;

    public ProfileController(ILogger<ProfileController> logger,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }
    
    [Route("/profile/logout")]
    public async Task<IActionResult> Logout()
    {
        await _currentUser.Logout();
        return Redirect("/");
    }
}