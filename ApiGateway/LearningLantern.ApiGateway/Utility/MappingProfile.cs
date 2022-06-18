using AutoMapper;
using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.Users.Commands;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.ApiGateway.Users.Models;

namespace LearningLantern.ApiGateway.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SignupCommand, UserModel>().ForMember(
            x => x.UserName,
            opt => opt.MapFrom(x => x.Email));
        CreateMap<UserModel, CreateUserEvent>();
        CreateMap<UserModel, UpdateUserEvent>();
        CreateMap<UserModel, UserDTO>().ReverseMap();
        CreateMap<ClassroomModel, AddClassroomDTO>().ReverseMap();
        CreateMap<ClassroomModel, ClassroomDTO>().ReverseMap();
    }
}