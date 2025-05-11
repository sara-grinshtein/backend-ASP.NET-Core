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
    public class MessageService : IService<MessageDto>
    {
        private readonly Irepository<Message> repository;
        private readonly IMapper mapper;

        public MessageService(Irepository<Message> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

        }

        public async Task< MessageDto> AddItem(MessageDto item)
        {
            return mapper.Map<Message,MessageDto>(await repository.AddItem(mapper.Map<MessageDto,Message>(item)));
        }

        public async Task DeleteItem(int id)
        {
           await repository.DeleteItem(id);
         }

        public async Task <List<MessageDto>> GetAll()
        {
            return mapper.Map<List<Message>, List<MessageDto>>( await repository.GetAll());
        }

        public async  Task <MessageDto> Getbyid(int id)
        {
            return mapper.Map<Message, MessageDto>(await repository.Getbyid(id));
        }

        public async Task UpDateItem(int id, MessageDto item)
        {
           await repository.UpDateItem(id, mapper.Map<MessageDto, Message>(item));

        }
    }


}
