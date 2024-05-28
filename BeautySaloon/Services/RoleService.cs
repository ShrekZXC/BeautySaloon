using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;

    
    public RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task<List<RoleModel>> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var rolesModel = _mapper.Map<List<RoleModel>>(roles);
        return rolesModel;
    }

    public async Task<string?> GetSelectedRole(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        if (appUser == null)
        {
            return string.Empty;
        }
        var roles = await _userManager.GetRolesAsync(appUser);
        var userRole = roles.FirstOrDefault();
        return userRole;

    }
}