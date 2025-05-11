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
using Azure;
using Response = Repository.Entites.Response;


namespace Service.service
{
    public class ResponseService : IService<ResponseDto>
    {

        private readonly Irepository<Response> repository;
        private readonly IMapper mapper;

        public ResponseService(Irepository<Response> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }
        public async Task<ResponseDto> AddItem(ResponseDto item)
        {
            return mapper.Map<Response, ResponseDto>(await repository.AddItem(mapper.Map<ResponseDto, Response>(item)));

        }

        public async Task DeleteItem(int id)
        {
           await repository.DeleteItem(id);
 
        }

        public async Task<List<ResponseDto>> GetAll()
        {
            return mapper.Map<List<Response>, List<ResponseDto>>(await repository.GetAll());
        }

        public async Task<ResponseDto> Getbyid(int id)
        {
            return mapper.Map<Response, ResponseDto>(await repository.Getbyid(id));
        }

        public async Task UpDateItem(int id, ResponseDto item)
        {
           await repository.UpDateItem(id, mapper.Map<ResponseDto, Response>(item));

        }
    }
}
