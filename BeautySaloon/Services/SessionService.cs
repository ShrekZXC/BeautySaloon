using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class SessionService : ISessionService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public SessionService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    public async Task<Guid> Create(SessionModel sessionModel)
    {
        var entity = _mapper.Map<SessionEntity>(sessionModel);
            
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
            
        return result;
    }

    public async Task<SessionModel> Get(Guid Id)
    {
        var entity = await _dbRepository.Get<SessionEntity>().FirstOrDefaultAsync(x => x.Id == Id);
        var sessionModel = _mapper.Map<SessionModel>(entity);

        return sessionModel;
    }

    public async Task Update(SessionModel sessionModel)
    {
        var entity = _mapper.Map<SessionEntity>(sessionModel);
            
        await _dbRepository.Update(entity);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task Delete(Guid sessionId)
    {
        var entity = await _dbRepository.Get<SessionEntity>().FirstOrDefaultAsync(x => x.Id == sessionId);
        if (entity != null) await _dbRepository.Remove<SessionEntity>(entity);
        await _dbRepository.SaveChangesAsync();
    }
}