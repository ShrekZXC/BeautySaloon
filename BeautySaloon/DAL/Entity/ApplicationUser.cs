using Microsoft.AspNetCore.Identity;

namespace BeautySaloon.DAL.Entity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
}