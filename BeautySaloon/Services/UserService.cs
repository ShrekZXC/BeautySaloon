using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Helpers;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BeautySaloon.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private SignInManager<ApplicationUser> _signInManager;

    public UserService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserModel userModel, string passsword)
    {
        var user = _mapper.Map<ApplicationUser>(userModel);
        user.UserName = GenerateUserName.Generate(user.FirstName, user.SecondName);
        var result = await _userManager.CreateAsync(user, passsword);
            
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        
        return result;
    }

    public async Task<SignInResult> Login(UserModel userModel, string password)
    {
        var user = _mapper.Map<ApplicationUser>(userModel);
        return await _signInManager.PasswordSignInAsync(user.Email, password, userModel.RememberMe, false);
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}