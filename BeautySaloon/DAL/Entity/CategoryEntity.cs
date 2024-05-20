namespace BeautySaloon.DAL.Entity;

public class CategoryEntity : BaseEntity
{
    public int CategoryId { get; set; }
    
    public string Name { get; set; }
    
    public string ImgSrc { get; set; }
}