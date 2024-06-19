using BeautySaloon.Model;
using BeautySaloon.ViewModel;

namespace BeautySaloon.Services.Interfaces;

public interface IServiceAppointmentService
{
    Task<List<ServiceAppointmentsModel>> GetAllServiceAppointments();
    
    Task<ServiceAppointmentsModel> GeServiceAppointmentById(Guid id);
    
    Task<List<WorkerModel>> GetAllWorkers();

    Task<List<ServiceAppointmentsModel>> GetServiceAppointmentsByWorkerIdAsync(Guid id);

    Task<ServiceAppointmentsModel> AddServiceAppointmentAsync(ServiceAppointmentsModel serviceAppointmentsModel);
    
    Task<ServiceAppointmentsModel> UpdateServiceAppointmentAsync(ServiceAppointmentsModel serviceAppointmentsModel);

    Task<List<ServiceAppointmentsModel>> GetAllServiceAppointmentsByClientId(Guid id);
    
    Task<List<ServiceAppointmentsModel>>  GetAllServiceAppointmentsByWorkerId(Guid id);

    Task<bool> DeleteServiceAppointmentById(Guid id);
}