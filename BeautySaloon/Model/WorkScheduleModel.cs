namespace BeautySaloon.Model;

public class WorkScheduleModel
{
    public Guid Id { get; set; }
    public Guid WorkerId { get; set; }
    public WorkerModel Worker { get; set; }
    
    public Guid ClientId { get; set; }
    
    public ClientModel Clien { get; set; }
    public DateTime WorkDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}