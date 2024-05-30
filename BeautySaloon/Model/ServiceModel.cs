namespace BeautySaloon.Model;

public class ServiceModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImageSrc { get; set; }
    
    public decimal Price { get; set; }
    
    public int Duration { get; set; } // Продолжительность в минутах
    
    public string CategoryId { get; set; }
    
    public CategoryModel Category { get; set; }
}