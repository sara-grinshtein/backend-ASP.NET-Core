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
            Console.WriteLine(" [AddItem] Starting...");

            try
            {
                Console.WriteLine(" [AddItem] Mapping VolunteerDto to Volunteer entity...");
                var volunteerEntity = mapper.Map<VolunteerDto, Volunteer>(item);
                var knowledgeDtos = item.areas_of_knowledge;
                volunteerEntity.areas_of_knowledge = null;

                Console.WriteLine(" [AddItem] Saving volunteer to repository...");
                var createdVolunteer = await repository.AddItem(volunteerEntity);

                if (createdVolunteer == null || createdVolunteer.volunteer_id == 0)
                    return null;

                if (knowledgeDtos != null && knowledgeDtos.Any())
                {
                    foreach (var dto in knowledgeDtos)
                    {
                        var knowledgeEntity = new My_areas_of_knowledge
                        {
                            volunteer_id = createdVolunteer.volunteer_id,
                            ID_knowledge = dto.ID_knowledge
                        };
                        await knowledge_repository.AddItem(knowledgeEntity);
                    }
                }

                var finalVolunteer = await repository.Getbyid(createdVolunteer.volunteer_id);
                return mapper.Map<Volunteer, VolunteerDto>(finalVolunteer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 [AddItem] Exception: {ex.Message}");
                throw;
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

        public async Task<VolunteerDto> UpDateItem(int id, VolunteerDto item)
        {
            try
            {
                Console.WriteLine($"🔄 [UpdateItem] Updating volunteer ID={id}...");
                var updated = await repository.UpDateItem(id, mapper.Map<VolunteerDto, Volunteer>(item));

                if (updated == null)
                {
                    Console.WriteLine("⚠️ [UpdateItem] Volunteer not found or not updated.");
                    return null;
                }

                return mapper.Map<Volunteer, VolunteerDto>(updated);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 [UpdateItem] Exception: {ex.Message}");
                return null;
            }
        }

        Task IService<VolunteerDto>.UpDateItem(int id, VolunteerDto item)
        {
            return UpDateItem(id, item);
        }
    }
}
