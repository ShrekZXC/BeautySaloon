using BeautySaloon.Model;

namespace BeautySaloon.Services.Interfaces;

public interface IServiceService
{
    Task<Guid> Create(ServiceModel serviceModel);

    Task<ServiceModel> Get(Guid serviceId);

    Task Update(ServiceModel serviceModel);

    Task Delete(Guid serviceId);
}