using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IPromotionService
{
    Task<Guid> Create(PromotionModel promoModel);

    Task<PromotionModel> Get(Guid promoId);

    Task<bool> Update(PromotionModel promoModel);

    List<PromotionModel> GetAll();

    Task Delete(Guid promoId);
}