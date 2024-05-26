using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class PromotionService : IPromotionService
{
    private readonly IDbRepository _dbRepository;
    private readonly IMapper _mapper;

    public PromotionService(IDbRepository dbRepository, IMapper mapper)
    {
        _dbRepository = dbRepository;
        _mapper = mapper;
    }
    
    
    public async Task<Guid> Create(PromotionModel promoModel)
    {
        var entity = _mapper.Map<PromotionEntity>(promoModel);
            
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
            
        return result;
    }

    public async Task<PromotionModel> Get(Guid promoId)
    {
        var entity = await _dbRepository.Get<PromotionEntity>().FirstOrDefaultAsync(x => x.Id == promoId);
        var serviceModel = _mapper.Map<PromotionModel>(entity);

        return serviceModel;
    }

    public async Task<bool> Update(PromotionModel promoModel)
    {
        try
        {
            var entity = _mapper.Map<PromotionEntity>(promoModel);
            
            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();
            
            return true;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }

    public List<PromotionModel> GetAll()
    {
        var entities =  _dbRepository.GetAll<PromotionEntity>();
        var promotionsModel = _mapper.Map<List<PromotionModel>>(entities).ToList();

        return promotionsModel;
    }

    public async Task Delete(Guid promoId)
    {
        var entity = await _dbRepository.Get<PromotionEntity>().FirstOrDefaultAsync(x => x.Id == promoId);
        if (entity != null) await _dbRepository.Remove<PromotionEntity>(entity);
        await _dbRepository.SaveChangesAsync();
    }
}