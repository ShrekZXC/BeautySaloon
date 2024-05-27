using BeautySaloon.Model;
using Microsoft.AspNetCore.Identity;

namespace BeautySaloon.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(UserModel userModel, string password);

    Task<SignInResult> Login(UserModel userModel, string password);
    
    Task Logout();
}