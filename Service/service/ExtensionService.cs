using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entites;
using Service.interfaces;
using Common.Dto;
using AutoMapper;
using Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

            services.AddAutoMapper(typeof(IMapper));

            return services;
        }
    }
}
