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

        public MessageDto AddItem(MessageDto item)
        {
            return mapper.Map<Message,MessageDto>(repository.AddItem(mapper.Map<MessageDto,Message>(item)));
        }

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
         }

        public List<MessageDto> GetAll()
        {
            return mapper.Map<List<Message>, List<MessageDto>>(repository.GetAll());
        }

        public MessageDto Getbyid(int id)
        {
            return mapper.Map<Message, MessageDto>(repository.Getbyid(id));
        }

        public void UpDateItem(int id, MessageDto item)
        {
            repository.UpDateItem(id, mapper.Map<MessageDto, Message>(item));

        }
    }


}
