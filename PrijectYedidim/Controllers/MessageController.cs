using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IService<MessageDto> service;

        // GET: api/<MessageController>
        [HttpGet]
        public List<MessageDto> GetAll()
        {
            return service.GetAll();
        }

        public MessageController(IService<MessageDto> service)
        {
            this.service = service;
        }

     
        

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public MessageDto Get(int id)
        {
            return service.Getbyid(id);
        }

        // POST api/<MessageController>
        [HttpPost]
        public MessageDto Post([FromBody] MessageDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] MessageDto value)
        {
            service.UpDateItem(id, value);
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
