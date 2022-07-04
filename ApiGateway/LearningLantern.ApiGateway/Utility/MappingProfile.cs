using AutoMapper;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.Commands;
using LearningLantern.ApiGateway.Users.Events;

namespace LearningLantern.ApiGateway.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SignupCommand, UserModel>().ForMember(
            x => x.UserName,
            opt => opt.MapFrom(x => x.Email));
        CreateMap<UserModel, UserEvent>();
        CreateMap<UserModel, UserDTO>().ReverseMap();
    }
}