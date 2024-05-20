namespace BeautySaloon.ViewModel;

public class UserViewModel
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    public string Salt { get; set; }
    
    public int IdRole { get; set; }
}