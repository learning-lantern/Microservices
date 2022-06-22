using AutoMapper;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddTaskDTO, TaskModel>()
            .ForMember(destinationMember => destinationMember.Id, memberOptions => memberOptions.Ignore())
            .ForMember(destinationMember => destinationMember.UserId, memberOptions => memberOptions.Ignore());
    }
}