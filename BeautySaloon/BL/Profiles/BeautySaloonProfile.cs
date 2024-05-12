﻿using AutoMapper;
using BeautySaloon.DAL.Entity;
using BeautySaloon.Model;
using BeautySaloon.ViewModel;

namespace BeautySaloon.BL.Profiles;

public class BeautySaloonProfile: Profile
{
    public BeautySaloonProfile()
    {
        CreateMap<SessionEntity, SessionModel>().ReverseMap();
        CreateMap<UserEntity, UserModel>().ReverseMap();
        CreateMap<UserTokenEntity, UserTokenModel>().ReverseMap();
        CreateMap<RegisterViewModel, UserModel>().ReverseMap();
    }
}