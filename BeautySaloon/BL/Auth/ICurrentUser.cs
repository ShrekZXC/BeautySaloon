namespace BeautySaloon.BL.Auth;

public interface ICurrentUser
{
    Task<bool> IsLoggedIn();

    Task<Guid?> GetCurrentUserId();
}