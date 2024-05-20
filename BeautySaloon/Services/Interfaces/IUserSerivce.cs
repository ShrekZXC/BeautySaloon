using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IUserSerivce
{
    Task<Guid> Create(UserModel user);

    Task<UserModel> Get(Guid Id);
    
    Task<UserModel> GetByEmail(string email);

    Task Update(UserModel user);

    Task Delete(Guid userId);

    Task<bool> IsAdmin(Guid userId);

    List<UserModel> GetAll();
}