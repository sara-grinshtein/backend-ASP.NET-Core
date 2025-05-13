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
        public async Task<VolunteerDto> AddItem(VolunteerDto item)
        {
            return mapper.Map<Volunteer, VolunteerDto>(await repository.AddItem(mapper.Map<VolunteerDto, Volunteer>(item)));

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

