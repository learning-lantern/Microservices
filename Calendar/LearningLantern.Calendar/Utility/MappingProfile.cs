using AutoMapper;
using LearningLantern.Common.Models.CalendarModels;

namespace LearningLantern.Calendar.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EventDTO, EventModel>().ForMember(
            x => x.Id, opt => opt.Ignore());
    }
}