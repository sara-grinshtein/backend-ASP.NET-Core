using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Dto;
using Service.interfaces;

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class My_areas_of_knowledge_Controller : ControllerBase
    {
        private readonly IService<KnowledgeCategoryDto> service;

        public My_areas_of_knowledge_Controller(IService<KnowledgeCategoryDto> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<KnowledgeCategoryDto>> GetAll()
        {
            return await service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<KnowledgeCategoryDto> GetAsync(int id)
        {
            return await service.Getbyid(id);
        }

        [HttpPost]
        public async Task<KnowledgeCategoryDto> PostAsync([FromBody] KnowledgeCategoryDto value)
        {
            return await service.AddItem(value);
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] KnowledgeCategoryDto value)
        {
            await service.UpDateItem(id, value);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await service.DeleteItem(id);
        }
    }
}
