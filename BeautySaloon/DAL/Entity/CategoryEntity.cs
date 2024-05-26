namespace BeautySaloon.DAL.Entity;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; }
    
    public string ImgSrc { get; set; }
    
    public ICollection<ServiceEntity> Services { get; set; }
}