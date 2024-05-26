namespace BeautySaloon.ViewModel;

public class ServiceViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageSrc { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public Guid CategoryId { get; set; }
    public List<CategoryViewModel> Categories { get; set; }
}