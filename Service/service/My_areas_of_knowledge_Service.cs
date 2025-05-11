using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.interfaces;
using Common.Dto;
using Repository.Entites;
using Service.interfaces;
using AutoMapper;
using Repository.interfaces;



namespace Service.service
{
    public class My_areas_of_knowledge_Service :IService<My_areas_of_knowledge_Dto>
    {
        private readonly Irepository<My_areas_of_knowledge> repository;
        private readonly IMapper mapper;


        public My_areas_of_knowledge_Service(Irepository<My_areas_of_knowledge> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }
        public async Task< My_areas_of_knowledge_Dto> AddItem(My_areas_of_knowledge_Dto item)
        {
            return mapper.Map<My_areas_of_knowledge, My_areas_of_knowledge_Dto>(await 
                repository.AddItem(mapper.Map<My_areas_of_knowledge_Dto, My_areas_of_knowledge>(item)));

        }

        public async Task DeleteItem(int id)
        {
           await repository.DeleteItem(id);
 
        }

        public async Task<List<My_areas_of_knowledge_Dto>> GetAll()
        {
            return mapper.Map<List<My_areas_of_knowledge>, List<My_areas_of_knowledge_Dto>>(await repository.GetAll());
        }

        public async Task<My_areas_of_knowledge_Dto> Getbyid(int id)
        {
            return mapper.Map<My_areas_of_knowledge, My_areas_of_knowledge_Dto>(await repository.Getbyid(id));

        }

        public async Task UpDateItem(int id, My_areas_of_knowledge_Dto item)
        {
          await repository.UpDateItem(id, mapper.Map<My_areas_of_knowledge_Dto, My_areas_of_knowledge>(item));

        }

        
    }
}
