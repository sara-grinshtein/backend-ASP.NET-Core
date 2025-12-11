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

            // KnowledgeCategory
            CreateMap<KnowledgeCategory, KnowledgeCategoryDto>().ReverseMap();

            // My_areas_of_knowledge → DTO
            CreateMap<My_areas_of_knowledge, My_areas_of_knowledge_Dto>()
                .ForMember(dest => dest.ID_knowledge, opt => opt.MapFrom(src => src.ID_knowledge))
                .ForMember(dest => dest.volunteer_id, opt => opt.MapFrom(src => src.volunteer_id))
                .ForMember(dest => dest.describtion, opt => opt.MapFrom(src =>
                    src.KnowledgeCategory != null ? src.KnowledgeCategory.describtion : string.Empty));

            // DTO → Entity
            CreateMap<My_areas_of_knowledge_Dto, My_areas_of_knowledge>()
                .ForMember(dest => dest.ID_knowledge, opt => opt.MapFrom(src => src.ID_knowledge))
                .ForMember(dest => dest.volunteer_id, opt => opt.MapFrom(src => src.volunteer_id))
                .ForMember(dest => dest.KnowledgeCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Volunteer, opt => opt.Ignore());

            // Volunteer → DTO
            CreateMap<Volunteer, VolunteerDto>()
                .ForMember(dest => dest.areas_of_knowledge, opt => opt.MapFrom(src => src.areas_of_knowledge));

            // VolunteerDto → Volunteer
            CreateMap<VolunteerDto, Volunteer>()
                .ForMember(dest => dest.areas_of_knowledge, opt => opt.MapFrom(src => src.areas_of_knowledge));

            // ✅ Response ↔ ResponseDto (המיפוי שהיה חסר וגרם ל-500)
            CreateMap<Response, ResponseDto>().ReverseMap();
        }
    }
}
