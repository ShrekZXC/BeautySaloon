namespace BeautySaloon.DAL.Entity;

public class AppointmentEntity
{
    public Guid ClientId { get; set; }
    public ApplicationUser Client { get; set; }
    
    public Guid MasterId { get; set; }
    public ApplicationUser Master { get; set; }
    
    public Guid ServiceId { get; set; }
    public ServiceEntity Service { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}