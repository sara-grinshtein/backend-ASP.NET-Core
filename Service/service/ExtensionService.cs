using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Service.interfaces;
using Common.Dto;
using AutoMapper;
using Service.Algorithm;
using Repository.Repositories;
using Repository.Entites;
using Microsoft.AspNetCore.Hosting;
using Common.Dto.Common.Dto;

namespace Service.service
{
    public static class ExtensionService
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddRepository();

            services.AddScoped<IService<MessageDto>, MessageService>();
            services.AddScoped<IService<VolunteerDto>, VolunteerService>();
            services.AddScoped<IService<HelpedDto>, HelpedService>();
            services.AddScoped<IService<ResponseDto>, ResponseService>();
            services.AddScoped<IService<My_areas_of_knowledge_Dto>, My_areas_of_knowledge_Service>();
            services.AddScoped<IService<KnowledgeCategoryDto>, KnowledgeCategoryService>();

            services.AddScoped<ICandidateScreening, Candidate_screening>();
            services.AddScoped<IDataFetcher, DataFetcher>();

            // 👇 הוספת מימוש זמני ל-IDistanceService
            services.AddScoped<IDistanceService, FakeDistanceService>();

            services.AddAutoMapper(typeof(IMapper));

            // הזרקת FilterService עם קובץ מילים אסורות מ-wwwroot
            services.AddScoped<FilterService>(provider =>
            {
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var filePath = Path.Combine(env.WebRootPath, "forbidden-words.txt");
                return new FilterService(filePath);
            });

            return services;
        }
    }

}
