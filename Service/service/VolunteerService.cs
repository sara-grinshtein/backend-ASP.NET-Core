using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Repository.Entites;
using Service.interfaces;
using Common.Dto;
using Repository.interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace Service.service
{
    public class VolunteerService : IService<VolunteerDto>
    {
        private readonly Irepository<Volunteer> repository;
        private readonly Irepository<My_areas_of_knowledge> knowledge_repository;
        private readonly IMapper mapper;



        public VolunteerService(Irepository<Volunteer> repository, IMapper mapper,
            Irepository<My_areas_of_knowledge> knowledge_repository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.knowledge_repository = knowledge_repository;

        }

        public async Task<VolunteerDto> AddItem(VolunteerDto item)
        {
            Console.WriteLine("🔄 [AddItem] Starting...");

            try
            {
                Console.WriteLine("🧭 [AddItem] Mapping VolunteerDto to Volunteer entity...");
                var volunteerEntity = mapper.Map<VolunteerDto, Volunteer>(item);
                var knowledgeDtos = item.areas_of_knowledge;
                volunteerEntity.areas_of_knowledge = null;

                Console.WriteLine("📥 [AddItem] Saving volunteer to repository...");
                var createdVolunteer = await repository.AddItem(volunteerEntity);

                if (createdVolunteer == null)
                {
                    Console.WriteLine("❌ [AddItem] repository.AddItem returned null.");
                    return null;
                }

                if (createdVolunteer.volunteer_id == 0)
                {
                    Console.WriteLine("❌ [AddItem] volunteer_id is 0 – likely not saved properly.");
                    return null;
                }

                Console.WriteLine($"✅ [AddItem] Volunteer saved: ID = {createdVolunteer.volunteer_id}");

                if (knowledgeDtos != null && knowledgeDtos.Any())
                {
                    Console.WriteLine($"📚 [AddItem] Adding {knowledgeDtos.Count} knowledge areas...");

                    foreach (var dto in knowledgeDtos)
                    {
                        var knowledgeEntity = new My_areas_of_knowledge
                        {
                            volunteer_id = createdVolunteer.volunteer_id,
                            ID_knowledge = dto.ID_knowledge // ✅ ודאי שזה קיים ב־dto
                        };


                        Console.WriteLine($"🧠 [AddItem] Saving knowledge area: '{dto.describtion}' for Volunteer ID {createdVolunteer.volunteer_id}");
                        await knowledge_repository.AddItem(knowledgeEntity);
                    }
                }
                else
                {
                    Console.WriteLine("ℹ️ [AddItem] No knowledge areas provided.");
                }

                Console.WriteLine("📦 [AddItem] Fetching volunteer from DB by ID...");
                var finalVolunteer = await repository.Getbyid(createdVolunteer.volunteer_id);

                if (finalVolunteer == null)
                {
                    Console.WriteLine("⚠️ [AddItem] Final fetch returned null.");
                    return null;
                }

                Console.WriteLine("🎯 [AddItem] Mapping final entity to DTO and returning.");
                return mapper.Map<Volunteer, VolunteerDto>(finalVolunteer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 [AddItem] Exception occurred: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"🧩 [AddItem] Inner exception: {ex.InnerException.Message}");

                return null;
            }
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);

        }

        public async Task<List<VolunteerDto>> GetAll()
        {
            return mapper.Map<List<Volunteer>, List<VolunteerDto>>(await repository.GetAll());
        }

        public async Task<VolunteerDto> Getbyid(int id)
        {
            return mapper.Map<Volunteer, VolunteerDto>(await repository.Getbyid(id));
        }

        public async Task UpDateItem(int id, VolunteerDto item)
        {
            await repository.UpDateItem(id, mapper.Map<VolunteerDto, Volunteer>(item));

        }

       

    }


}

