using AutoMapper;
using LearningLantern.Common.Models.TodoModels;

namespace LearningLantern.TodoList.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskDTO, TaskModel>().ForMember(
            x => x.Id, opt => opt.Ignore());
    }
}