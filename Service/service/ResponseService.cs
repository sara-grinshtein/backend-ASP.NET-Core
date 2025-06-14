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
        private readonly Irepository<Message> messageRepository;


        public ResponseService(Irepository<Response> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.messageRepository = messageRepository;  


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

        public async Task<List<Message>> GetMessagesUserRespondedToAsync(int helpedId)
        {
            // שולפים את כל התגובות של המשתמש כולל ה־Message
            var responses = await repository.GetAll();
            var filtered = responses
                .Where(r => r.helped_id == helpedId)
                .DistinctBy(r => r.message_id)
                .ToList();

            // את לא משתמשת ב־Include כי את לא עובדת ישירות עם EF כאן
            // לכן נדרש שירות אחר שיביא את ההודעות לפי ID
            // אם את כן משתמשת ב־EF מאחורי הקלעים — עדיף להעביר את זה ל־Repository

            // כרגע הפונקציה מחזירה רק את IDs של ההודעות
            // אפשר לשנות את זה בהתאם למה את מחזירה מה־Repository
            return filtered
                .Select(r => r.Message) // רק אם ב־Response יש navigation property בשם Message
                .ToList();
        }



    }
}
