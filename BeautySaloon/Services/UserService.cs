using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.BL.General;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Exception;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class UserService : IUserSerivce
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;
    private readonly IEncrypt _encrypt;
    private readonly IDbSession _dbSession;

    public UserService(IDbRepository dbRepository, 
        IMapper mapper,
        IEncrypt encrypt,
        IDbSession dbSession)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
        _encrypt = encrypt;
        _dbSession = dbSession;
    }
    
    public async Task ValidateEmail(string email)
    {
        var user = await GetByEmail(email);
        if (user != null)
            throw new DuplicateEmailException();
    }
    
    public async Task<Guid> Create(UserModel userModel)
    {
        var entity = _mapper.Map<UserEntity>(userModel);
        
        using (var scope = Helpers.CreateTransactionScope())
        {
            await ValidateEmail(entity.Email);
            scope.Complete();
        }
        
        entity.Salt = Guid.NewGuid().ToString();
        entity.Password = _encrypt.HashPassword(entity.Password, entity.Salt);
        
        var result = await _dbRepository.Add(entity);
        await _dbSession.SetUserId(result);
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
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x=>x.Id == userId);
        if (user != null) await _dbRepository.Remove<UserEntity>(user);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task<UserModel> GetByEmail(string email)
    {
        var entity = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Email == email);
        var userModel = _mapper.Map<UserModel>(entity);

        return userModel;
    }
}