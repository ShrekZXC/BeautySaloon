namespace BeautySaloon.DAL.Entity;

public class ServiceAppointmentsEntity : BaseEntity
{
    public Guid WorkerId { get; set; }
    public DateTime WorkDate { get; set; }
    public Guid ClientId { get; set; }
    public Guid ServiceId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public ApplicationUser Worker { get; set; }
    public ApplicationUser Client { get; set; }
    public ServiceEntity Service { get; set; }
}