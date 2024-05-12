using BeautySaloon.BL.General;
using BeautySaloon.DAL;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;

namespace BeautySaloon.BL.Auth;

public class DbSession : IDbSession
{
    private readonly IWebCookie _webCookie;
    private readonly ISessionService _sessionService;
    
    private SessionModel? sessionModel = null;

    public DbSession(IWebCookie webCookie, ISessionService sessionService)
    {
        _webCookie = webCookie;
        _sessionService = sessionService;
    }
    
    private async Task<SessionModel> CreateSession()
    {
        var data = new SessionModel()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.Now,
            LastAccessed = DateTime.Now
        };
        await _sessionService.Create(data);
        return data;
    }
    
    private void CreateSessionCookie(Guid sessionid)
    {
        _webCookie.Delete(AuthConstants.SessionCookieName);
        _webCookie.AddSecure(AuthConstants.SessionCookieName, sessionid.ToString());
    }
    
    public async Task<SessionModel> GetSession()
    {
        if (sessionModel != null)
            return sessionModel;

        Guid sessionId;
        var sessionString = _webCookie.Get(AuthConstants.SessionCookieName);
        if (sessionString != null)
            sessionId = Guid.Parse(sessionString);
        else
            sessionId = Guid.NewGuid();

        var data = await _sessionService.Get(sessionId);
        if (data == null)
        {
            data = await this.CreateSession();
            CreateSessionCookie(data.Id);
        }
        sessionModel = data;
        return data;
    }
    
    public async Task SetUserId(Guid userId)
    {
        var data = await this.GetSession();
        data.UserId = userId;
        data.Id = Guid.NewGuid();
        CreateSessionCookie(data.Id);
        await _sessionService.Create(data);
    }

    public async Task<bool> IsLoggedIn()
    {
        var data = await this.GetSession();
        return data.UserId != null;
    }
    
    public async Task<Guid?> GetUserId()
    {
        var data = await this.GetSession();
        return data.UserId;
    }
    
    public void ResetSessionCache()
    {
        sessionModel = null;
    }
    
    public async Task DeleteSessionId()
    {
        await GetSession();
        if (this.sessionModel != null)
            await _sessionService.Delete(sessionModel.Id);
    }
}