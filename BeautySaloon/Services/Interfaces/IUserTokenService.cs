using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IUserTokenService
{
    Task<Guid> Create(UserTokenModel userTokenModel);

    Task<UserTokenModel> Get(Guid Id);

    Task Update(UserTokenModel userTokenModel);

    Task Delete(Guid Id);
}