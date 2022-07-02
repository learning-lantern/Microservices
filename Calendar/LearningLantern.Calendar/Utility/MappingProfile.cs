using AutoMapper;
using LearningLantern.Calendar.Data.Models;

namespace LearningLantern.Calendar.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EventModel, EventDTO>();
        
        CreateMap<AddEventDTO, EventModel>().ForMember(
            x => x.Id, opt => opt.Ignore());
    }
}