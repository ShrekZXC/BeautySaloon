using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.ViewModel;

namespace BeautySaloon.BL;

public class BeautySaloonProfile: Profile
{
    public BeautySaloonProfile()
    {
        CreateMap<ServiceModel, ServiceViewModel>().ReverseMap();
        CreateMap<ServiceModel, ServiceEntity>().ReverseMap();
        CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
        CreateMap<CategoryModel, CategoryViewModel>().ReverseMap();
        CreateMap<PromotionEntity, PromotionModel>().ReverseMap();
        CreateMap<PromotionModel, PromotionViewModel>().ReverseMap();
        CreateMap<UserViewModel, UserModel>().ReverseMap();
        CreateMap<UserModel, ApplicationUser>().ReverseMap();
        CreateMap<RegisterViewModel, UserModel>().ReverseMap();
        CreateMap<LoginViewModel, UserModel>().ReverseMap();
        CreateMap<RoleModel, ApplicationRole>().ReverseMap();
        CreateMap<RoleViewModel, RoleModel>().ReverseMap();
    }
}