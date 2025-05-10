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
        public HelpedDto AddItem(HelpedDto item)
        {
            return mapper.Map<Helped, HelpedDto>(repository.AddItem(mapper.Map<HelpedDto, Helped>(item)));

        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
             
        }

        public List<HelpedDto> GetAll()
        {
            return mapper.Map<List<Helped>, List<HelpedDto>>(repository.GetAll());
        }

        public HelpedDto Getbyid(int id)
        {
            return mapper.Map<Helped, HelpedDto>(repository.Getbyid(id));
        }

        public void UpDateItem(int id, HelpedDto item)
        {
            repository.UpDateItem(id, mapper.Map<HelpedDto, Helped>(item));

        }
    }
}
