namespace BeautySaloon.Model;

public class SessionModel
{
    public Guid Id { get; set; }
    public string? SessionContent { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastAccessed { get; set; }
    public Guid? UserId { get; set; }
}