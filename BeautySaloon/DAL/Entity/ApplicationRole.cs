using Microsoft.AspNetCore.Identity;

namespace BeautySaloon.DAL.Entity;


public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    // Добавьте любые дополнительные свойства здесь
}
