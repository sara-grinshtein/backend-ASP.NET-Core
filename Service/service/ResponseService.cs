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
        public ResponseDto AddItem(ResponseDto item)
        {
            return mapper.Map<Response, ResponseDto>(repository.AddItem(mapper.Map<ResponseDto, Response>(item)));

        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
 
        }

        public List<ResponseDto> GetAll()
        {
            return mapper.Map<List<Response>, List<ResponseDto>>(repository.GetAll());
        }

        public ResponseDto Getbyid(int id)
        {
            return mapper.Map<Response, ResponseDto>(repository.Getbyid(id));
        }

        public void UpDateItem(int id, ResponseDto item)
        {
            repository.UpDateItem(id, mapper.Map<ResponseDto, Response>(item));

        }
    }
}
