using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IServiceService
{
    Task<Guid> Create(ServiceModel serviceModel);

    Task<ServiceModel> Get(Guid serviceId);

    Task Update(ServiceModel serviceModel);

    List<ServiceModel> GetAll();

    Task Delete(Guid serviceId);
}