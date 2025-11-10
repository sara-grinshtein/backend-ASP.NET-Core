using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IService<VolunteerDto> service;

        public VolunteerController(IService<VolunteerDto> service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<VolunteerDto>> GetAll()
        {
            Console.WriteLine(" התקבלה בקשת GET לכל המתנדבים");
            var result = await service.GetAll();
            Console.WriteLine($" כמות מתנדבים שהוחזרה: {result.Count}");
            return result;
        }

        [HttpGet("{id}")]
        public async Task<VolunteerDto> GetAsync(int id)
        {
            Console.WriteLine($" התקבלה בקשת GET למתנדב לפי מזהה: {id}");
            var volunteer = await service.Getbyid(id);
            Console.WriteLine($" מתנדב שנמצא: {volunteer?.volunteer_first_name} {volunteer?.volunteer_last_name}");
            return volunteer;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] VolunteerDto value)
        {
            Console.WriteLine(" התקבלה בקשת POST ליצירת מתנדב חדש");
            Console.WriteLine($" אימייל: {value.email},  שם: {value.volunteer_first_name} {value.volunteer_last_name}");
            var added = await service.AddItem(value);
            if(added== null)
            {
                Console.WriteLine("יצירת מתנדב נכשלה (service החזיר null)");
                return StatusCode(500, "failed to create volunteer");
            }
                

            Console.WriteLine($" מתנדב נוצר עם מזהה: {added.volunteer_id}");
            return CreatedAtAction(nameof(GetAsync), new { id = added.volunteer_id }, added);
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] VolunteerDto value)
        {
            Console.WriteLine($"✏️ התקבלה בקשת PUT לעדכון מתנדב עם מזהה: {id}");
            Console.WriteLine($"📍 מיקום חדש: Latitude = {value.Latitude}, Longitude = {value.Longitude}");
            Console.WriteLine($"📧 אימייל: {value.email}, 📛 שם: {value.volunteer_first_name} {value.volunteer_last_name}");

            await service.UpDateItem(id, value);

            Console.WriteLine(" העדכון הושלם");
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            Console.WriteLine($" התקבלה בקשת DELETE למחיקת מתנדב עם מזהה: {id}");
            await service.DeleteItem(id);
            Console.WriteLine(" המתנדב נמחק בהצלחה");
        }
    }
}
