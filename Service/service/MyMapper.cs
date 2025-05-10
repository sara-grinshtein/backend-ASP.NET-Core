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

            // Knowledge Areas
            CreateMap<My_areas_of_knowledge, My_areas_of_knowledge_Dto>().ReverseMap();

            // Response
            CreateMap<Response, ResponseDto>().ReverseMap();

            // Volunteer
            CreateMap<Volunteer, VolunteerDto>().ReverseMap();
        }
    }
}
