namespace BeautySaloon.DAL.Entity
{
    public class FooterSettingsEntity : BaseEntity
    {
        public string SocialMediaName { get; set; }
        public string SocialMediaLink { get; set; }
        public string FooterColor { get; set; }
        public string WorkingHours { get; set; }
        public string BackgroundImage { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}