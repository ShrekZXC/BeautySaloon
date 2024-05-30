namespace BeautySaloon.DAL.Entity;

public class ScheduleEntity : BaseEntity
{
    public Guid WorkerId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<SlotEntity> Slots { get; set; }
}