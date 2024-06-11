namespace BeautySaloon.DAL.Entity;

public class MainSettingsEntity : BaseEntity
{
    // Header
    public string? SiteName { get; set; }
    
    public string? ColorBackgroundHeader { get; set; }
    
    public string? ColorTextHeader { get; set; }
    
    public string? BackgroundImageHeader { get; set; }
    
    // Body
    public string? MainText { get; set; }
    
    public string? ColorMainText { get; set; }
    
    public string? ColorBackgroundMain { get; set; }
    
    public string? MainBackgroundImage { get; set; }
    
    // Footer
    public string? ColorFooterText { get; set; }
    
    public string? BackgroundImageFooter { get; set; }
    
    public string? ColorBackgroundFooter { get; set; }
}