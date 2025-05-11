using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Repository.Entites;
using Service.interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpedController : ControllerBase
    {

        private readonly IService<HelpedDto> service;

        [HttpGet]
        public List<HelpedDto> GetAll()
        {
            return service.GetAll();
        }
        public HelpedController(IService<HelpedDto> service)
        {
            this.service = service;
        }
      
        // GET api/<HelpedController>/5
        [HttpGet("{id}")]
        public HelpedDto Get(int id)
        {
             return  service.Getbyid(id);
        }
      

        // POST api/<HelpedController>
        [HttpPost]
        public HelpedDto Post([FromBody]HelpedDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<HelpedController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]HelpedDto value)
        {
            service.UpDateItem(id, value);
        }

        // DELETE api/<HelpedController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
