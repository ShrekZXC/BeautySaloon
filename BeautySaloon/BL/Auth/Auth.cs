using BeautySaloon.BL.General;
using BeautySaloon.Exception;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;

namespace BeautySaloon.BL.Auth;

public class Auth : IAuth
{
    private readonly IUserSerivce _userSerivce;
    private readonly IEncrypt _encrypt;
    private readonly IDbSession _dbSession;
    private readonly IUserTokenService _userTokenService;
    private readonly IWebCookie _webCookie;
    
    public Auth(IUserSerivce userSerivce,
        IEncrypt encrypt,
        IDbSession dbSession,
        IUserTokenService userTokenService,
        IWebCookie webCookie)
    {
        _userSerivce = userSerivce;
        _encrypt = encrypt;
        _dbSession = dbSession;
        _userTokenService = userTokenService;
        _webCookie = webCookie;
    }
    
    public async Task Login(Guid id)
    {
        await _dbSession.SetUserId(id);
    }
    
    public async Task<Guid> Authenticate(string email, string password, bool rememberMe)
    {
        var user = await _userSerivce.GetByEmail(email);

        if (user != null && user.Password == _encrypt.HashPassword(password, user.Salt))
        {
            await Login(user.Id);

            if (rememberMe)
            {
                Guid tokenId = await _userTokenService.Create(user.Id);
                _webCookie.AddSecure(AuthConstants.RememberMeCookieName, tokenId.ToString(), AuthConstants.RememberMeDays);
            }

            return user.Id;
        }
        throw new AuthorizationException();
    }
}