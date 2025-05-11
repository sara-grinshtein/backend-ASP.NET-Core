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

namespace Service.service
{
    public class HelpedService : IService<HelpedDto>
    {

        private readonly Irepository<Helped> repository;
        private readonly IMapper mapper;


        public HelpedService(Irepository<Helped> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }
        public async  Task<HelpedDto> AddItem(HelpedDto item)
        {
            return mapper.Map<Helped, HelpedDto>(await repository.AddItem(mapper.Map<HelpedDto, Helped>(item)));

        }

        public async Task DeleteItem(int id)
        {
           await repository.DeleteItem(id);
             
        }

        public async Task<List<HelpedDto>> GetAll()
        {
            return mapper.Map<List<Helped>, List<HelpedDto>>(await repository.GetAll());
        }

        public async Task<HelpedDto> Getbyid(int id)
        {
            return mapper.Map<Helped, HelpedDto>(await repository.Getbyid(id));
        }

        public async Task UpDateItem(int id, HelpedDto item)
        {
           await repository.UpDateItem(id, mapper.Map<HelpedDto, Helped>(item));

        }
    }
}
