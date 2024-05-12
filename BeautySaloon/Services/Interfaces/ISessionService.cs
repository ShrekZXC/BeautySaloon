using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface ISessionService
{
    Task<Guid> Create(SessionModel sessionModel);

    Task<SessionModel> Get(Guid Id);

    Task Update(SessionModel sessionModel);

    Task Delete(Guid sessionId);
}