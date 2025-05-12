using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class My_areas_of_knowledge_Controller : ControllerBase
    {
        private readonly IService<My_areas_of_knowledge_Dto> service;
        [HttpGet]
        public async Task<List<My_areas_of_knowledge_Dto>> GetAll()
        {
            return await service.GetAll();
        }
        public My_areas_of_knowledge_Controller(IService<My_areas_of_knowledge_Dto> service)
        {
            this.service = service;
        }


        // GET api/<My_areas_of_knowledge_Controller>/5
        [HttpGet("{id}")]
        public async Task<My_areas_of_knowledge_Dto> GetAsync(int id)
        {
            return await service.Getbyid(id);
        }

        // POST api/<My_areas_of_knowledge_Controller>
        [HttpPost]
        public async Task<My_areas_of_knowledge_Dto> PostAsync([FromBody] My_areas_of_knowledge_Dto value)
        {
            return await service.AddItem(value);
        }

        // PUT api/<My_areas_of_knowledge_Controller>/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] My_areas_of_knowledge_Dto value)
        {
            await service.UpDateItem(id, value);
        }

        // DELETE api/<My_areas_of_knowledge_Controller>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await service.DeleteItem(id);
        }
    }
}
