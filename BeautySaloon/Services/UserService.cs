using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class UserService : IUserSerivce
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public UserService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    public async Task<Guid> Create(UserModel userModel)
    {
        var entity = _mapper.Map<UserEntity>(userModel);
        
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
            
        return result;
    }

    public async Task<UserModel> Get(Guid Id)
    {
        var entity = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == Id);
        var userModel = _mapper.Map<UserModel>(entity);

        return userModel;
    }

    public async Task Update(UserModel userModel)
    {
        var entity = _mapper.Map<UserEntity>(userModel);
            
        await _dbRepository.Update(entity);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task Delete(Guid userId)
    {
        await _dbRepository.Delete<UserEntity>(userId);
        await _dbRepository.SaveChangesAsync();
    }
}