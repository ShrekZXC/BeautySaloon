using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class UserTokenService : IUserTokenService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public UserTokenService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    public async Task<Guid> Create(Guid userId)
    {
        Guid tockenId = Guid.NewGuid();
        var entity = new UserTokenEntity();
        entity.Id = tockenId;
        entity.UserId = userId;
            
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
            
        return result;
    }

    public async Task<UserTokenModel> Get(Guid Id)
    {
        var entity = await _dbRepository.Get<UserTokenEntity>().FirstOrDefaultAsync(x => x.Id == Id);
        var sessionModel = _mapper.Map<UserTokenModel>(entity);

        return sessionModel;
    }

    public async Task Update(UserTokenModel userTokenModel)
    {
        var entity = _mapper.Map<UserTokenEntity>(userTokenModel);
            
        await _dbRepository.Update(entity);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task Delete(Guid sessionId)
    {
        var entity = await _dbRepository.Get<UserTokenEntity>().FirstOrDefaultAsync(x => x.Id == sessionId);
        if (entity != null) await _dbRepository.Remove<UserTokenEntity>(entity);
        await _dbRepository.SaveChangesAsync();
    }
}