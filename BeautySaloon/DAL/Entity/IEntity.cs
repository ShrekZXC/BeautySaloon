namespace BeautySaloon.DAL.Entity;

public interface IEntity
{
    Guid Id { get; set; }
    
    bool IsActive { get; set; }
}