namespace BeautySaloon.DAL.Entity;

public class SessionEntity : BaseEntity
{
    public string? SessionContent { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastAccessed { get; set; }
    public int? UserId { get; set; }
}