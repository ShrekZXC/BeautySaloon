namespace BeautySaloon.ViewModel;

public class ScheduleViewModel
{
    public Guid Id { get; set; }
    public Guid MasterId { get; set; }
    public WorkerViewModel Master { get; set; }
}