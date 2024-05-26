using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IRoleService
{
    Task<Guid> Create(RoleModel roleModel);

    Task<RoleModel> Get(Guid roleId);

    Task Update(RoleModel roleModel);

    List<RoleModel> GetAll();

    Task Delete(Guid roleId);
}