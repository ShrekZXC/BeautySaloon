using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;

namespace BeautySaloon.BL.Auth;

public interface IDbSession
{
    Task<SessionModel> GetSession();
    
    Task<bool> IsLoggedIn();
    
    Task SetUserId(Guid userId);
    
    Task<Guid?> GetUserId();

    Task DeleteSessionId();

    void ResetSessionCache();
}