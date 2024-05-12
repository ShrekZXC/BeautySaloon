using BeautySaloon.BL.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.ViewComponents;

public class AuthViewComponent: ViewComponent 
{
    private readonly ICurrentUser currentUser;

    public AuthViewComponent(ICurrentUser currentUser)
    {
        this.currentUser = currentUser;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        bool isLoggedIn = await currentUser.IsLoggedIn();
        return View("Index", isLoggedIn);
    }
}