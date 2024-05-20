namespace BeautySaloon.DAL.Entity;

public class RoleEntity : BaseEntity
{
    public int idRole { get; set; }
    public string Role { get; set; }
    
    public ICollection<UserEntity> Users { get; set; }
}