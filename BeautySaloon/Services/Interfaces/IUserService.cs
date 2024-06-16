using BeautySaloon.Model;
using Microsoft.AspNetCore.Identity;

namespace BeautySaloon.Services.Interfaces;

public interface IUserService
{
    Task<UserModel?> FindByIdAsync(Guid id);
    
    Task<List<UserModel>> GetAllUsers();
    
    Task<List<WorkerModel>> GetAllWorkers();
    
    Task<IdentityResult?> UpdateUser(UserModel userModel);
    
    Task<IdentityResult> DeleteUser(Guid id);
    
    Task<IdentityResult> RegisterUserAsync(UserModel userModel, string password, string? roleName, bool isSigin = true);
    
    Task<SignInResult> Login(UserModel userModel, string password);
    
    Task<List<ClientModel>> GetAllClients();
    
    Task Logout();
}