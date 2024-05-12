namespace BeautySaloon.Model;

public class UserModel
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Phone { get; set; }
    
    public string Salt { get; set; }
}