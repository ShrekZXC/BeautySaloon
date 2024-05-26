using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;

namespace BeautySaloon.Services;

public class RoleService : IRoleService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public RoleService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    public Task<Guid> Create(RoleModel roleModel)
    {
        throw new NotImplementedException();
    }

    public Task<RoleModel> Get(Guid roleId)
    {
        throw new NotImplementedException();
    }

    public Task Update(RoleModel roleModel)
    {
        throw new NotImplementedException();
    }

    public List<RoleModel> GetAll()
    {
        var entities =  _dbRepository.GetAll<RoleEntity>();
        var rolesModel = _mapper.Map<List<RoleModel>>(entities).ToList();

        return rolesModel;
    }

    public Task Delete(Guid roleId)
    {
        throw new NotImplementedException();
    }
}