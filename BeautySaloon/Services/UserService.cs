using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Helpers;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<UserModel?> FindByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        if (appUser == null)
        {
            return null;
        }

        var user = _mapper.Map<UserModel>(appUser);

        return user;
    }
    
    
    public async Task<IdentityResult?> UpdateUser(UserModel userModel)
    {
        var appUser = await _userManager.FindByIdAsync(userModel.Id.ToString());
        if (appUser == null)
        {
            return null;
        }

        appUser.FirstName = userModel.FirstName;
        appUser.SecondName = userModel.SecondName;
        appUser.LastName = userModel.LastName;
        appUser.Email = userModel.Email;
        appUser.PhoneNumber = userModel.PhoneNumber;
        
        var result = await _userManager.UpdateAsync(appUser);
        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(appUser);
            await _userManager.RemoveFromRolesAsync(appUser, roles);
            await _userManager.AddToRoleAsync(appUser, userModel.SelectedRole);
        }

        return result;
    }

    public async Task<IdentityResult> DeleteUser(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return null;
        }
        var result = await _userManager.DeleteAsync(user);
        return result;
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        var appUsers = await _userManager.Users.ToListAsync();
        var users = _mapper.Map<List<UserModel>>(appUsers);
        foreach (var user in appUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            users.FirstOrDefault(x=>x.Id == user.Id)!.SelectedRole = roles.FirstOrDefault()!;
        }
        return users;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserModel userModel, string passsword, string? roleName, bool isSigin = true)
    {
        var user = _mapper.Map<ApplicationUser>(userModel);
        user.UserName = GenerateUserName.Generate(user.FirstName, user.SecondName);
        var result = await _userManager.CreateAsync(user, passsword);
            
        if (result.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                return IdentityResult.Failed(roleResult.Errors.ToArray());
            }

            if (isSigin)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
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