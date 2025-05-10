using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository.Entites;
using Repository.interfaces;

namespace Repository.Repositories
{
    public static class ExtensionRepository
    {
        public static IServiceCollection  AddRepository( this IServiceCollection services)
        {
             services.AddScoped<Irepository<Message>, MessageRpository>();
             
            services.AddScoped<Irepository< Volunteer>, VolunteerRpository>();

            services.AddScoped<Irepository<Response>, ResponsRepository>();

            services.AddScoped<Irepository<My_areas_of_knowledge>, My_areas_of_knowledgeRepository>();

            services.AddScoped<Irepository<Helped>, HelpedRepository>();  

            return services;
        }

    }
}
