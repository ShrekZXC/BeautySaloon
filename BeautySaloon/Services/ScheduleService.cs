using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using BeautySaloon.Model;
using BeautySaloon.Services.Interfaces;
using BeautySaloon.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Services;

public class ScheduleService : IScheduleService
{
    private readonly IDbRepository _dbRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    
    public ScheduleService(
        UserManager<ApplicationUser> userManager, 
        IMapper mapper, IDbRepository dbRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _dbRepository = dbRepository;
    }

    public async Task<WorkScheduleModel> GetWorkScheduleById(Guid id)
    {
        var entity = await _dbRepository.Get<WorkScheduletEntity>().FirstOrDefaultAsync(x => x.Id == id);
        var workScheduleModel = _mapper.Map<WorkScheduleModel>(entity);
        return workScheduleModel;
    }

    public async Task<List<WorkerModel>> GetAllWorkers()
    {
        var workers = new List<WorkerModel>();
        var users = _userManager.Users.ToList();

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, "Worker"))
            {
                // Добавляем пользователя в список работников
                workers.Add(_mapper.Map<WorkerModel>(user));
            }
        }

        return workers;
    }

    public async Task<List<WorkScheduleModel>> GetWorkSchedulesAsync(Guid id)
    {
        var workScheduletEntities = await _dbRepository.Get<WorkScheduletEntity>()
            .Where(ws => ws.WorkerId == id)
            .Include(x=>x.Worker)
            .Include(x=>x.Client)
            .Include(x=>x.Service)
            .ToListAsync();

        return _mapper.Map<List<WorkScheduleModel>>(workScheduletEntities);
    }

    public async Task<WorkScheduleModel> AddWorkScheduleAsync(WorkScheduleModel workScheduleModel)
    {
        var entity = _mapper.Map<WorkScheduletEntity>(workScheduleModel);
        var id = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();

        workScheduleModel = new WorkScheduleModel();
        var workScheduletEntity = await _dbRepository
            .Get<WorkScheduletEntity>()
            .Include(x=>x.Worker)
            .Include(x=>x.Client)
            .Include(x=>x.Service)
            .FirstOrDefaultAsync(x => x.Id == id);
        workScheduleModel = _mapper.Map<WorkScheduleModel>(workScheduletEntity);
        return workScheduleModel;
    }

    public async Task UpdateWorkScheduleAsync(WorkScheduleModel workScheduleModel)
    {
        var entity = await _dbRepository.Get<WorkScheduletEntity>()
            .FirstOrDefaultAsync(x=>x.Id == workScheduleModel.Id);

        if (entity != null)
        {
            entity.StartTime = workScheduleModel.StartTime;
            entity.EndTime = workScheduleModel.EndTime;

            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();
        }
    }
}