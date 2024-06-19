using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class ServiceAppointmentService : IServiceAppointmentService
{
    private readonly IDbRepository _dbRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    
    public ServiceAppointmentService(
        UserManager<ApplicationUser> userManager, 
        IMapper mapper, IDbRepository dbRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _dbRepository = dbRepository;
    }

    public async Task<List<ServiceAppointmentsModel>> GetAllServiceAppointments()
    {
        var serviceAppointments = await
            _dbRepository
                .GetAll<ServiceAppointmentsEntity>()
                .Include(x=>x.Service)
                .Include(x=>x.Client)
                .Include(x=>x.Worker)
                .ToListAsync();

        var getAllServiceAppointmentsModel = _mapper.Map<List<ServiceAppointmentsModel>>(serviceAppointments);
        
        return getAllServiceAppointmentsModel;
    }

    public async Task<ServiceAppointmentsModel> GeServiceAppointmentById(Guid id)
    {
        var entity = await _dbRepository
            .Get<ServiceAppointmentsEntity>()
            .Include(x=>x.Service)
            .Include(x=>x.Client)
            .Include(x=>x.Worker)
            .FirstOrDefaultAsync(x => x.Id == id);
        var workScheduleModel = _mapper.Map<ServiceAppointmentsModel>(entity);
        return workScheduleModel;
    }

    public async Task<List<ServiceAppointmentsModel>> GetAllServiceAppointmentsByClientId(Guid id)
    {
        var workScheduletEntities = await _dbRepository.Get<ServiceAppointmentsEntity>()
            .Where(ws => ws.ClientId == id)
            .Include(x=>x.Worker)
            .Include(x=>x.Client)
            .Include(x=>x.Service)
            .ToListAsync();

        return _mapper.Map<List<ServiceAppointmentsModel>>(workScheduletEntities);
    }
    
    public async Task<List<ServiceAppointmentsModel>> GetAllServiceAppointmentsByWorkerId(Guid id)
    {
        var workScheduletEntities = await _dbRepository.Get<ServiceAppointmentsEntity>()
            .Where(ws => ws.WorkerId == id)
            .Include(x => x.Worker)
            .Include(x => x.Client)
            .Include(x => x.Service)
            .ToListAsync();

        return _mapper.Map<List<ServiceAppointmentsModel>>(workScheduletEntities);
    }

    public async Task<bool> DeleteServiceAppointmentById(Guid id)
    {
        try
        {
            var schedule = await _dbRepository.Get<ServiceAppointmentsEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (schedule != null)
            {
                await _dbRepository.Remove<ServiceAppointmentsEntity>(schedule);
                await _dbRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }

    public async Task<List<WorkerModel>> GetAllWorkers()
    {
        var workers = new List<WorkerModel>();
        var users = _userManager.Users.ToList();

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, "User"))
            {
                continue;
            }
            // Добавляем пользователя в список работников
            workers.Add(_mapper.Map<WorkerModel>(user));
        }

        return workers;
    }

    public async Task<List<ServiceAppointmentsModel>> GetServiceAppointmentsByWorkerIdAsync(Guid id)
    {
        var workScheduletEntities = await _dbRepository.Get<ServiceAppointmentsEntity>()
            .Where(ws => ws.WorkerId == id)
            .Include(x=>x.Worker)
            .Include(x=>x.Client)
            .Include(x=>x.Service)
            .ToListAsync();

        return _mapper.Map<List<ServiceAppointmentsModel>>(workScheduletEntities);
    }

    public async Task<ServiceAppointmentsModel> AddServiceAppointmentAsync(ServiceAppointmentsModel serviceAppointmentsModel)
    {
        var entity = _mapper.Map<ServiceAppointmentsEntity>(serviceAppointmentsModel);
        var id = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();

        serviceAppointmentsModel = new ServiceAppointmentsModel();
        var workScheduletEntity = await _dbRepository
            .Get<ServiceAppointmentsEntity>()
            .Include(x=>x.Worker)
            .Include(x=>x.Client)
            .Include(x=>x.Service)
            .FirstOrDefaultAsync(x => x.Id == id);
        serviceAppointmentsModel = _mapper.Map<ServiceAppointmentsModel>(workScheduletEntity);
        return serviceAppointmentsModel;
    }

    public async Task<ServiceAppointmentsModel> UpdateServiceAppointmentAsync(ServiceAppointmentsModel serviceAppointmentsModel)
    {
        var entity = await _dbRepository.Get<ServiceAppointmentsEntity>()
            .FirstOrDefaultAsync(x=>x.Id == serviceAppointmentsModel.Id);

        if (entity != null)
        {
            entity.ClientId = serviceAppointmentsModel.ClientId;
            entity.ServiceId = serviceAppointmentsModel.ServiceId;
            entity.StartTime = serviceAppointmentsModel.StartTime;
            entity.EndTime = serviceAppointmentsModel.EndTime;

            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();
            
            var workScheduletEntity = await _dbRepository
                .Get<ServiceAppointmentsEntity>()
                .Include(x=>x.Worker)
                .Include(x=>x.Client)
                .Include(x=>x.Service)
                .FirstOrDefaultAsync(x => x.Id == serviceAppointmentsModel.Id);
            serviceAppointmentsModel = _mapper.Map<ServiceAppointmentsModel>(workScheduletEntity);
            return serviceAppointmentsModel;
        }
        else
        {
            return null;
        }
    }
}