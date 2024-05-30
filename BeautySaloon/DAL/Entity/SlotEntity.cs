﻿namespace BeautySaloon.DAL.Entity;

public class SlotEntity : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public Guid ScheduleId { get; set; }
    public ScheduleEntity Schedule { get; set; }
}