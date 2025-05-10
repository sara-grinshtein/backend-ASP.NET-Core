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
    public class VolunteerService : IService<VolunteerDto>
    {
        private readonly Irepository<Volunteer> repository;
        private readonly IMapper mapper;
         


        public VolunteerService(Irepository<Volunteer> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }
        public VolunteerDto AddItem(VolunteerDto item)
        {
            return mapper.Map<Volunteer, VolunteerDto>(repository.AddItem(mapper.Map<VolunteerDto, Volunteer>(item)));

        }
        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
 
        }

        public List<VolunteerDto> GetAll()
        {
            return mapper.Map<List<Volunteer>, List<VolunteerDto>>(repository.GetAll());
        }

        public VolunteerDto Getbyid(int id)
        {
            return mapper.Map<Volunteer, VolunteerDto>(repository.Getbyid(id));
        }

        public void UpDateItem(int id, VolunteerDto item)
        {
            repository.UpDateItem(id, mapper.Map<VolunteerDto, Volunteer>(item));

        }
    }


}
