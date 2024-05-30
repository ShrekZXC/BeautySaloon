using BeautySaloon.Model;
using BeautySaloon.ViewModel;

namespace BeautySaloon.Services.Interfaces;

public interface IScheduleService
{
    Task<WorkScheduleModel> GetWorkScheduleById(Guid id);
    Task<List<WorkerModel>> GetAllWorkers();

    Task<List<WorkScheduleModel>> GetWorkSchedulesAsync(Guid id);

    Task<Guid> AddWorkScheduleAsync(WorkScheduleModel workScheduleModel);
    
    Task UpdateWorkScheduleAsync(WorkScheduleModel workScheduleModel);
}