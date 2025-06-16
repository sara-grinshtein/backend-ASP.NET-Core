using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Authorization;
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

        public MessageController(IService<MessageDto> service)
        {
            this.service = service;
        }

        // GET: api/<MessageController>
        [HttpGet]
        public async Task<List<MessageDto>> GetAllAsync()
        {
            return await service.GetAll();
        }


        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public async Task<MessageDto> Get(int id)
        {
            return await service.Getbyid(id);
        }

        // POST api/<MessageController>
        [HttpPost]
        public async Task<MessageDto> Post([FromBody] MessageDto value)
        {

            Console.WriteLine($"POST received: helped_id={value.helped_id}, volunteer_id={value.volunteer_id}");
            return await service.AddItem(value);
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] MessageDto value)
        {
            await service.UpDateItem(id, value);
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
        }


        [Authorize]
        [HttpGet("my-messages")]
        public async Task<List<MessageDto>> GetByHelpedId()
        {
            var helpedIdClaim = User.FindFirst("userid")?.Value;
            if (helpedIdClaim == null)
                return new List<MessageDto>();

            int helpedId = int.Parse(helpedIdClaim);

            var allMessages = await service.GetAll();
            return allMessages.Where(m => m.helped_id == helpedId).ToList();
        }
 

    }
}
