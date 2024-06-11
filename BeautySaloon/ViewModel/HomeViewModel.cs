using BeautySaloon.DAL.Entity;

namespace BeautySaloon.ViewModel;

public class HomeViewModel
{
    public List<PromotionViewModel> PromotionsViewModel { get; set; }
    
    public List<CategoryViewModel> CategoriesViewModel { get; set; }
    
    public MainSettingsEntity? MainSettings { get; set; }
}