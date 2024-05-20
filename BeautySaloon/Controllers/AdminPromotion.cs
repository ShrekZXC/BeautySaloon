using AutoMapper;
using BeautySaloon.BL.Auth;
using BeautySaloon.Services.Interfaces;

namespace BeautySaloon.Controllers;

public class AdminPromotion : AdminBaseController
{
    public AdminPromotion(ILogger<AdminBaseController> logger,
        ICurrentUser currentUser,
        IUserSerivce userService,
        IMapper mapper) : 
        base(logger, currentUser, userService, mapper)
    {
    }
}