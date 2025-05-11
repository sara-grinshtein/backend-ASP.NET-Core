using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IService<VolunteerDto> service;
        [HttpGet]
        public async Task<List<VolunteerDto>> GetAll()
        {
            return await service.GetAll();
        }
        public VolunteerController(IService<VolunteerDto> service)
        {
            this.service = service;
        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        public VolunteerDto Get(int id)
        {
            return service.Getbyid(id);
        }

        // POST api/<VolunteerController>
        [HttpPost]
        public VolunteerDto Post([FromBody] VolunteerDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<VolunteerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] VolunteerDto value)
        {
            service.UpDateItem(id, value);
        }

        // DELETE api/<VolunteerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
