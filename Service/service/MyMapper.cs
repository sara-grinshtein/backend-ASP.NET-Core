using AutoMapper;
using Common.Dto;
using Common.Dto.Common.Dto;
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

            // KnowledgeCategory -> DTO
            CreateMap<KnowledgeCategory, KnowledgeCategoryDto>()
                .ForMember(dest => dest.ID_knowledge, opt => opt.MapFrom(src => src.ID_knowledge))
                .ForMember(dest => dest.describtion, opt => opt.MapFrom(src => src.describtion));

            // DTO -> KnowledgeCategory
            CreateMap<KnowledgeCategoryDto, KnowledgeCategory>()
                .ForMember(dest => dest.ID_knowledge, opt => opt.MapFrom(src => src.ID_knowledge))
                .ForMember(dest => dest.describtion, opt => opt.MapFrom(src => src.describtion));

            // My_areas_of_knowledge → DTO כולל תיאור מהקטגוריה
            CreateMap<My_areas_of_knowledge, My_areas_of_knowledge_Dto>()
                .ForMember(dest => dest.ID_knowledge, opt => opt.MapFrom(src => src.ID_knowledge))
                .ForMember(dest => dest.volunteer_id, opt => opt.MapFrom(src => src.volunteer_id))
                .ForMember(dest => dest.describtion, opt => opt.MapFrom(src => src.KnowledgeCategory.describtion));

            // DTO → Entity
            CreateMap<My_areas_of_knowledge_Dto, My_areas_of_knowledge>()
                .ForMember(dest => dest.ID_knowledge, opt => opt.MapFrom(src => src.ID_knowledge))
                .ForMember(dest => dest.volunteer_id, opt => opt.MapFrom(src => src.volunteer_id))
                .ForMember(dest => dest.KnowledgeCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Volunteer, opt => opt.Ignore());

            // Response
            CreateMap<Response, ResponseDto>().ReverseMap();

            // Volunteer
            CreateMap<Volunteer, VolunteerDto>()
                .ForMember(dest => dest.areas_of_knowledge, opt => opt.MapFrom(src => src.areas_of_knowledge));

            CreateMap<VolunteerDto, Volunteer>()
                .ForMember(dest => dest.areas_of_knowledge, opt => opt.Ignore());
        }
    }
}
