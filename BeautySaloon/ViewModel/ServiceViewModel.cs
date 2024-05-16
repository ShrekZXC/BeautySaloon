namespace BeautySaloon.ViewModel;

public class ServiceViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }
    public string ImageSrc { get; set; }
    public decimal Price { get; set; }
}