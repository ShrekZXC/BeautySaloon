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
    private readonly Guid AdminId = new Guid("8c1e6b9f-7c66-4594-8f81-a5dffce769de");
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
        // Установите RoleId на нужное значение, например, для роли "Пользователь"
        entity.RoleId = new Guid("425abb27-6970-41dc-8e54-ae8ffe3c3e0e");  // Замените на соответствующий GUID роли

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
    
    public List<UserModel> GetAll()
    {
        var entity =  _dbRepository.GetAll<UserEntity>().Include(x=>x.Role);
        var users = _mapper.Map<List<UserModel>>(entity).ToList();

        return users;
    }

    public async Task<bool> Update(UserModel userModel)
    {
        try
        {
            var entity = _mapper.Map<UserEntity>(userModel);
            
            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();

            return true;
        }
        catch (System.Exception ex)
        {
            return false;
        }
    }

    public async Task Delete(Guid userId)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x=>x.Id == userId);
        if (user != null) await _dbRepository.Remove<UserEntity>(user);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task<bool> IsAdmin(Guid userId)
    {        
        var entity = await _dbRepository.Get<UserEntity>()
            .Include(x=>x.Role)
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        return entity != null && entity.Role.Id == AdminId;
    }

    public async Task<UserModel> GetByEmail(string email)
    {
        var entity = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Email == email);
        var userModel = _mapper.Map<UserModel>(entity);

        return userModel;
    }
}