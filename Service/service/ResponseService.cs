using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Repository.Entites;
using Repository.interfaces;
using Service.interfaces;
using Service.service;

namespace Service.service
{
    public class ResponseService : IService<ResponseDto>
    {
        private readonly Irepository<Response> repository;
        private readonly IMapper mapper;
        private readonly Irepository<Message> messageRepository;
        private readonly FilterService filterService;

        public ResponseService(
            Irepository<Response> repository,
            IMapper mapper,
            Irepository<Message> messageRepository,
            FilterService filterService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.messageRepository = messageRepository;
            this.filterService = filterService;
        }

        public async Task<ResponseDto> AddItem(ResponseDto item)
        {
            if (!string.IsNullOrWhiteSpace(item.context))
            {
                var forbiddenWords = filterService.GetForbiddenWordsInText(item.context);
                if (forbiddenWords.Any())
                {
                    var joined = string.Join(", ", forbiddenWords.Distinct());
                    throw new ArgumentException($"התגובה כוללת מילים אסורות: {joined}");
                }

                item.context = filterService.FilterText(item.context);
            }

            var entity = mapper.Map<ResponseDto, Response>(item);
            var added = await repository.AddItem(entity);
            return mapper.Map<Response, ResponseDto>(added);
        }

        public async Task DeleteItem(int id) => await repository.DeleteItem(id);

        public async Task<List<ResponseDto>> GetAll() =>
            mapper.Map<List<Response>, List<ResponseDto>>(await repository.GetAll());

        public async Task<ResponseDto> Getbyid(int id) =>
            mapper.Map<Response, ResponseDto>(await repository.Getbyid(id));

        public async Task UpDateItem(int id, ResponseDto item) =>
            await repository.UpDateItem(id, mapper.Map<ResponseDto, Response>(item));

        public async Task<int?> GetHelpedIdByResponseIdAsync(int responseId)
        {
            var response = await repository.Getbyid(responseId);
            if (response == null) return null;

            var message = await messageRepository.Getbyid(response.message_id);
            return message?.helped_id;
        }
    }
}
