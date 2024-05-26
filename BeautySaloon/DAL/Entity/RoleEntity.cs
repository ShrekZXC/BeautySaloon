namespace BeautySaloon.DAL.Entity;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<UserEntity> Users { get; set; }
}