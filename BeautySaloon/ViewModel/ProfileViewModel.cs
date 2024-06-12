namespace BeautySaloon.ViewModel;

public class ProfileViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    
    public List<ServiceAppointmentsViewModel> Appointments { get; set; }
}