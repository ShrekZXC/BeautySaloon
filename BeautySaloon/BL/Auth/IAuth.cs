namespace BeautySaloon.BL.Auth;

public interface IAuth
{
    Task<Guid> Authenticate(string email, string password, bool rememberMe);

    Task Login(Guid Id);
}