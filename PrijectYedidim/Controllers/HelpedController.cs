using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Repository.Entites;
using Service.interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpedController : ControllerBase
    {

        private readonly IService<HelpedDto> service;
        public HelpedController(IService<HelpedDto> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<HelpedDto>> GetAll()
        {
            return await service.GetAll();
        }
       
        // GET api/<HelpedController>/5
        [HttpGet("{id}")]
        public async Task<HelpedDto> Get(int id)
        {
             return await service.Getbyid(id);
        }
        [HttpPost]
       // [Authorize]
        public async Task<HelpedDto> Post([FromBody]HelpedDto value)
        {
            return await service.AddItem(value);
        }
        
        // PUT api/<HelpedController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]HelpedDto value)
        {
          await  service.UpDateItem(id, value);
        }

        // DELETE api/<HelpedController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
          await  service.DeleteItem(id);
        }
    }
}
