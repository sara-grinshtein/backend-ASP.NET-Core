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
        public List<My_areas_of_knowledge_Dto> GetAll()
        {
            return service.GetAll();
        }
        public My_areas_of_knowledge_Controller(IService<My_areas_of_knowledge_Dto> service)
        {
            this.service = service;
        }
        

        // GET api/<My_areas_of_knowledge_Controller>/5
        [HttpGet("{id}")]
        public My_areas_of_knowledge_Dto Get(int id)
        {
            return service.Getbyid(id);
        }

        // POST api/<My_areas_of_knowledge_Controller>
        [HttpPost]
        public My_areas_of_knowledge_Dto Post([FromBody] My_areas_of_knowledge_Dto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<My_areas_of_knowledge_Controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] My_areas_of_knowledge_Dto value)
        {
            service.UpDateItem(id, value);
        }

        // DELETE api/<My_areas_of_knowledge_Controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
