using BeautySaloon.BL.General;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;

namespace BeautySaloon.BL.Auth;

public class CurrentUser: ICurrentUser
{
    private readonly IDbSession _dbSession;
    private readonly IWebCookie _webCookie;
    private readonly IUserTokenService _userTokenService;

    public CurrentUser(
        IDbSession dbSession,
        IWebCookie webCookie,
        IUserTokenService userTokenService
    )
    {
        _dbSession = dbSession;
        _webCookie = webCookie;
        _userTokenService = userTokenService;
    }

    public async Task<Guid?> GetUserIdByToken()
    {
        string? tokenCookie = _webCookie.Get(AuthConstants.RememberMeCookieName);
        if (tokenCookie == null)
            return null;
        Guid? tokenGuid = Helpers.StringToGuidDef(tokenCookie ?? "");
        if (tokenGuid == null)
            return null;

        var userToken = await _userTokenService.Get((Guid)tokenGuid);
        return userToken.UserId;
    }

    public async Task<bool> IsLoggedIn()
    {
        bool isLoggedIn = await _dbSession.IsLoggedIn();
        if (!isLoggedIn)
        {
            Guid? userid = await GetUserIdByToken();
            if (userid != null)
            {
                await _dbSession.SetUserId((Guid)userid);
                isLoggedIn = true;
            }
        }
        return isLoggedIn;
    }

    public async Task<Guid?> GetCurrentUserId()
    {
        return await _dbSession.GetUserId();
    }
}