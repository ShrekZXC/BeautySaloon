namespace BeautySaloon.ViewModel;

public class AppointmentViewModel
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public UserViewModel Client { get; set; }
    public Guid MasterId { get; set; }
    public UserViewModel Master { get; set; }
    public Guid ServiceId { get; set; }
    public ServiceViewModel Service { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}