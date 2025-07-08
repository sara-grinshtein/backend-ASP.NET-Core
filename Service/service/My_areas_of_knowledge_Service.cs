using AutoMapper;
using Common.Dto.Common.Dto;
using Repository.Entites;
using Repository.interfaces;
using Service.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.service
{
    public class My_areas_of_knowledge_Service : IService<My_areas_of_knowledge_Dto>
    {
        private readonly Irepository<My_areas_of_knowledge> repository;
        private readonly IMapper mapper;

        public My_areas_of_knowledge_Service(Irepository<My_areas_of_knowledge> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<My_areas_of_knowledge_Dto> AddItem(My_areas_of_knowledge_Dto item)
        {
            var entity = mapper.Map<My_areas_of_knowledge_Dto, My_areas_of_knowledge>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<My_areas_of_knowledge, My_areas_of_knowledge_Dto>(added);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<My_areas_of_knowledge_Dto>> GetAll()
        {
            var all = await repository.GetAll();
            return mapper.Map<List<My_areas_of_knowledge>, List<My_areas_of_knowledge_Dto>>(all);
        }

        public async Task<My_areas_of_knowledge_Dto> Getbyid(int id)
        {
            var item = await repository.Getbyid(id);
            return mapper.Map<My_areas_of_knowledge, My_areas_of_knowledge_Dto>(item);
        }

        public async Task UpDateItem(int id, My_areas_of_knowledge_Dto item)
        {
            var entity = mapper.Map<My_areas_of_knowledge_Dto, My_areas_of_knowledge>(item);
            await repository.UpDateItem(id, entity);
        }

        // ✅ פונקציה חדשה – שליפת כל תחומי הידע של מתנדב
       

        public async Task<List<My_areas_of_knowledge_Dto>> GetByVolunteerId(int volunteerId)
        {
            var list = await repository.GetAll(); // או תשתמש בשאילתה ממוקדת אם יש
            return mapper.Map<List<My_areas_of_knowledge>, List<My_areas_of_knowledge_Dto>>(
                list.Where(x => x.volunteer_id == volunteerId).ToList()
            );
        }

    }
}
