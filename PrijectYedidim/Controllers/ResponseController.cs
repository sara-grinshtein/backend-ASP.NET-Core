using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IService<ResponseDto> service;

        public ResponseController(IService<ResponseDto> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<ResponseDto>> GetAllAsync()
        {
            return await service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ResponseDto> GetAsync(int id)
        {
            return await service.Getbyid(id);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ResponseDto value)
        {
            try
            {
                var result = await service.AddItem(value);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] ResponseDto value)
        {
            try
            {
                await service.UpDateItem(id, value);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await service.DeleteItem(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("public")]
        public async Task<List<ResponseDto>> GetPublicResponses()
        {
            var all = await service.GetAll();
            return all.Where(r => r.isPublic).ToList();
        }
    }
}
