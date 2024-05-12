namespace BeautySaloon.DAL.Entity;

public class UserTokenEntity : BaseEntity
{
    public Guid UserId { get; set; }
    
    public DateTime Created { get; set; }
}