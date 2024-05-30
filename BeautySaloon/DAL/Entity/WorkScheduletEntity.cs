namespace BeautySaloon.DAL.Entity;

public class WorkScheduletEntity : BaseEntity
{
    public Guid WorkerId { get; set; }
    public DateTime WorkDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public ApplicationUser Worker { get; set; }
}