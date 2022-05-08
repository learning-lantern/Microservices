using AutoMapper;
using LearningLantern.TodoList.Data.Models;

namespace LearningLantern.TodoList.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskDTO, TaskModel>().ForMember(
            x => x.Id, opt => opt.Ignore());
    }
}