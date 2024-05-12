namespace BeautySaloon.DAL.Entity;

public class BaseEntity : IEntity
{
    public Guid Id { get; set; }
    
    public bool IsActive { get; set; }
}