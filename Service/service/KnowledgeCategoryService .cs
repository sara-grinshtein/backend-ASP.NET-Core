using AutoMapper;
using Common.Dto;
using Repository.Entites;
using Repository.interfaces;
using Service.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.service
{
    public class KnowledgeCategoryService : IService<KnowledgeCategoryDto>
    {
        private readonly Irepository<KnowledgeCategory> _repo;
        private readonly IMapper _mapper;

        public KnowledgeCategoryService(Irepository<KnowledgeCategory> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<KnowledgeCategoryDto> AddItem(KnowledgeCategoryDto item)
        {
            var entity = _mapper.Map<KnowledgeCategory>(item);
            var result = await _repo.AddItem(entity);
            return _mapper.Map<KnowledgeCategoryDto>(result);
        }

        public async Task DeleteItem(int id)
        {
            await _repo.DeleteItem(id);
        }

        public async Task<List<KnowledgeCategoryDto>> GetAll()
        {
            var list = await _repo.GetAll();
            return _mapper.Map<List<KnowledgeCategoryDto>>(list);
        }

        public async Task<KnowledgeCategoryDto> Getbyid(int id)
        {
            var result = await _repo.Getbyid(id);
            return _mapper.Map<KnowledgeCategoryDto>(result);
        }

        public async Task UpDateItem(int id, KnowledgeCategoryDto item)
        {
            var entity = _mapper.Map<KnowledgeCategory>(item);
            await _repo.UpDateItem(id, entity);
        }
    }
}
