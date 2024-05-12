using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class ServiceService : IServiceService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public ServiceService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    public async Task<Guid> Create(ServiceModel serviceModel)
    {
        var entity = _mapper.Map<ServiceEntity>(serviceModel);
            
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
            
        return result;
    }

    public async Task<ServiceModel> Get(Guid serviceId)
    {
        var entity = await _dbRepository.Get<ServiceEntity>().FirstOrDefaultAsync(x => x.Id == serviceId);
        var serviceModel = _mapper.Map<ServiceModel>(entity);

        return serviceModel;
    }

    public async Task Update(ServiceModel serviceModel)
    {
        var entity = _mapper.Map<ServiceEntity>(serviceModel);
            
        await _dbRepository.Update(entity);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task Delete(Guid serviceId)
    {
        var entity = await _dbRepository.Get<ServiceEntity>().FirstOrDefaultAsync(x => x.Id == serviceId);
        if (entity != null) await _dbRepository.Remove<ServiceEntity>(entity);
        await _dbRepository.SaveChangesAsync();
    }
}