namespace BeautySaloon.DAL.Entity;

public class ServiceEntity : BaseEntity
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string ImageSrc { get; set; }
    
    public decimal Price { get; set; }
    
    public int IdCatorgory { get; set; }
}