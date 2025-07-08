using Microsoft.AspNetCore.Mvc;
using Common.Dto.Common.Dto;
using Service.service;
using Service.interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class My_areas_of_knowledge_Controller : ControllerBase
    {
        private readonly My_areas_of_knowledge_Service _service;

        public My_areas_of_knowledge_Controller(My_areas_of_knowledge_Service service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<My_areas_of_knowledge_Dto>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<My_areas_of_knowledge_Dto> GetAsync(int id)
        {
            return await _service.Getbyid(id);
        }

        [HttpPost]
        public async Task<My_areas_of_knowledge_Dto> PostAsync([FromBody] My_areas_of_knowledge_Dto value)
        {
            return await _service.AddItem(value);
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] My_areas_of_knowledge_Dto value)
        {
            await _service.UpDateItem(id, value);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _service.DeleteItem(id);
        }

        // ✅ פונקציה חדשה – שליפת תחומי ידע לפי מזהה מתנדב

        [HttpGet("volunteer/{volunteerId}")]
        public async Task<List<My_areas_of_knowledge_Dto>> GetByVolunteerId(int volunteerId)
        {
            return await _service.GetByVolunteerId(volunteerId);
        }

    }
}
