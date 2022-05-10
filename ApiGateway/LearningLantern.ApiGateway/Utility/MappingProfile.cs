using AutoMapper;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;

namespace LearningLantern.ApiGateway.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClassroomModel, AddClassroomDTO>().ReverseMap();
        CreateMap<ClassroomModel, ClassroomDTO>().ReverseMap();
    }
}