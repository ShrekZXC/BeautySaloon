using BeautySaloon.Model;

namespace BeautySaloon.ViewModel;

public class WorkScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid WorkerId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ServiceId { get; set; }
    public DateTime WorkDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public WorkerViewModel Worker { get; set; }
    public ClientViewModel Client { get; set; }
    public ServiceViewModel Service { get; set; }
}