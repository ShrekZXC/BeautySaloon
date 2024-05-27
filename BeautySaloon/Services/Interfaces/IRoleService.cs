using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IRoleService
{
    Task<List<RoleModel>> GetAllRoles();
}