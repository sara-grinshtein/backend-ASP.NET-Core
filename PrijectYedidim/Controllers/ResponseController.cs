using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IService<ResponseDto> service;
        [HttpGet]
        public List<ResponseDto> GetAll()
        {
            return service.GetAll();
        }
        public ResponseController(IService<ResponseDto> service)
        {
            this.service = service;
        }
    
        // GET api/<ResponseController>/5
        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            return service.Getbyid(id);
        }

        // POST api/<ResponseController>
        [HttpPost]
        public ResponseDto Post([FromBody] ResponseDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<ResponseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ResponseDto value)
        {
            service.UpDateItem(id, value);
        }

        // DELETE api/<ResponseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
