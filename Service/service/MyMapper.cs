using AutoMapper;
using Common.Dto;
using Repository.Entites;

namespace Service.service
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            // Helped
            CreateMap<Helped, HelpedDto>().ReverseMap();

            // Message
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Volunteer, opt => opt.Ignore())
                .ForMember(dest => dest.Helped, opt => opt.Ignore());

            // Knowledge Areas
            CreateMap<My_areas_of_knowledge, My_areas_of_knowledge_Dto>()
                .ReverseMap()
                .ForMember(dest => dest.Volunteer, opt => opt.Ignore())
                .ForMember(dest => dest.volunteer_id, opt => opt.Ignore()); // חשוב – לא נכתוב volunteer_id ידנית

            // Response
            CreateMap<Response, ResponseDto>().ReverseMap();

            // Volunteer
            CreateMap<Volunteer, VolunteerDto>()
                .ForMember(dest => dest.areas_of_knowledge, opt => opt.MapFrom(src => src.areas_of_knowledge));

            CreateMap<VolunteerDto, Volunteer>()
                .ForMember(dest => dest.areas_of_knowledge, opt => opt.Ignore()); // לא למפות אוטומטית – נטפל ידנית בשירות
        }
    }

}
