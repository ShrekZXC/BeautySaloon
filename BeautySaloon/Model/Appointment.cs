namespace BeautySaloon.Model;

public class Appointment
{
    public Guid ClientId { get; set; }
    public UserModel Client { get; set; }
    public Guid MasterId { get; set; }
    public UserModel Master { get; set; }
    public Guid ServiceId { get; set; }
    public ServiceModel Service { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}