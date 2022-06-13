using AutoMapper;
using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Models;

namespace LearningLantern.ApiGateway.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SignupDTO, UserModel>().ForMember(
            x => x.UserName,
            opt => opt.MapFrom(x => x.Email));
        CreateMap<UserModel, UserDTO>().ReverseMap();
        CreateMap<ClassroomModel, AddClassroomDTO>().ReverseMap();
        CreateMap<ClassroomModel, ClassroomDTO>().ReverseMap();
    }
}