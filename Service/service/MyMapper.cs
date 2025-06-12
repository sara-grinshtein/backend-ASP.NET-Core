using AutoMapper;
using Common.Dto;
using Repository.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CreateMap<My_areas_of_knowledge, My_areas_of_knowledge_Dto>().ReverseMap();

            // Response
            CreateMap<Response, ResponseDto>()
           .ForMember(dest => dest.message_id, opt => opt.MapFrom(src => src.message_id))
           .ReverseMap()
           .ForMember(dest => dest.message_id, opt => opt.MapFrom(src => src.message_id));


            // Volunteer
            CreateMap<Volunteer, VolunteerDto>().ReverseMap();

        }
    }
}
