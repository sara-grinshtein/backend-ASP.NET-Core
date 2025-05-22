using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using Service.service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IService<ResponseDto> service;
        [HttpGet]
        public async Task<List<ResponseDto>> GetAllAsync()
        {
            return await service.GetAll();
        }
        public ResponseController(IService<ResponseDto> service)
        {
            this.service = service;
        }

        // GET api/<ResponseController>/5
        [HttpGet("{id}")]
        public async Task<ResponseDto> GetAsync(int id)
        {
            return await service.Getbyid(id);
        }

        // POST api/<ResponseController>
        [HttpPost]
        public async Task<ResponseDto> PostAsync([FromBody] ResponseDto value)
        {
            return await service.AddItem(value);
        }

        // PUT api/<ResponseController>/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] ResponseDto value)
        {
            await service.UpDateItem(id, value);
        }

        // DELETE api/<ResponseController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await service.DeleteItem(id);
        }


        // כדי שהמתנדב יראה רק תגובות לקריאות שנשלחו אליו
        //[HttpGet("by-volunteer/{volunteerId}")]
        //public async Task<List<ResponseDto>> GetByVolunteer(int volunteerId)
        //{
        //    // כאן נדרשת גישה ל־MessageService כדי לבדוק למי שייכת כל קריאה
        //    // אם כבר הזרקת אותו בקונסטרקטור, השתמשי בו — אם לא, נוסיף עכשיו
        //    var allMessages = await service.GetAll();
        //    var messageIds = allMessages
        //        .Where(m => m.volunteer_id == volunteerId)
        //        .Select(m => m.message_id)
        //        .ToList();

        //    var allResponses = await service.GetAll();
        //    return allResponses
        //        .Where(r => messageIds.Contains(r.me)) // message_id חייב להיות ב־ResponseDto
        //        .ToList();
        //}

        // כדי שהנעזר יראה רק את התגובות ששייכות לו
        [HttpGet("by-helped/{helpedId}")]
        public async Task<List<ResponseDto>> GetByHelped(int helpedId)
        {
            var allResponses = await service.GetAll();
            return allResponses
                .Where(r => r.helped_id == helpedId)
                .ToList();
        }


    }
}
