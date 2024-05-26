namespace BeautySaloon.DAL.Entity;

public class PromotionEntity: BaseEntity
{
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImgSrc { get; set; }
}