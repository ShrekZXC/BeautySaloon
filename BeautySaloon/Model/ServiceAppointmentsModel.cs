namespace BeautySaloon.Model;

public class ServiceAppointmentsModel
{
    public Guid Id { get; set; }
    public Guid WorkerId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ServiceId { get; set; }
    public WorkerModel Worker { get; set; }
    public ClientModel Client { get; set; }
    public ServiceModel Service { get; set; }
    public DateTime WorkDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}