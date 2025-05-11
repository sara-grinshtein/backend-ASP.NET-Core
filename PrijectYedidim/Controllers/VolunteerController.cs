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
        public async Task<VolunteerDto> GetAsync(int id)
        {
            return await service.Getbyid(id);
        }

        // POST api/<VolunteerController>
        [HttpPost]
        public async Task<VolunteerDto> PostAsync([FromBody] VolunteerDto value)
        {
            return await service.AddItem(value);
        }

        // PUT api/<VolunteerController>/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] VolunteerDto value)
        {
            await service.UpDateItem(id, value);
        }

        // DELETE api/<VolunteerController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await service.DeleteItem(id);
        }
    }
}
