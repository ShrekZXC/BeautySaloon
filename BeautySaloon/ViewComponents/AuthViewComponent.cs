using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.ViewComponents;

public class AuthViewComponent: ViewComponent 
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserSerivce _userSerivce;
    private readonly IMapper _mapper;

    public AuthViewComponent(ICurrentUser currentUser,
        IUserSerivce userSerivce,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _userSerivce = userSerivce;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        bool isLoggedIn = await _currentUser.IsLoggedIn();
        if (isLoggedIn == true)
        {
            var userId = await _currentUser.GetCurrentUserId();
            var user = await _userSerivce.Get((Guid)userId!);
            var profileViewModel = _mapper.Map<ProfileViewModel>(user);
            return View("Index", profileViewModel);
        }
        return View("Index");
    }
}