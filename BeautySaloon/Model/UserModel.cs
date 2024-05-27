namespace BeautySaloon.Model;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string SelectedRole { get; set; }
    public bool RememberMe { get; set; }
    public List<RoleModel> Roles { get; set; }
}