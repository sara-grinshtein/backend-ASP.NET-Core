
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using Service.Algorithm;

namespace PrijectYedidim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IService<MessageDto> service;
        private readonly IService<VolunteerDto> volunteerService;
        private readonly IEmailService emailService;
        private readonly ManagerAlgorithm assigner;

        public MessageController(
            IService<MessageDto> service,
            IService<VolunteerDto> volunteerService,
            IEmailService emailService,
            ManagerAlgorithm assigner)
        {
            this.service = service;
            this.volunteerService = volunteerService;
            this.emailService = emailService;
            this.assigner = assigner;
        }

        [HttpGet]
        public async Task<List<MessageDto>> GetAllAsync()
        {
            return await service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<MessageDto> Get(int id)
        {
            return await service.Getbyid(id);
        }

        [HttpPost]
        public async Task<MessageDto> Post([FromBody] MessageDto value)
        {
            Console.WriteLine($"POST received: helped_id={value.helped_id}, volunteer_id={value.volunteer_id}");

            var message = await service.AddItem(value);

            // ✅ הרצת אלגוריתם שיבוץ
            assigner.AssignVolunteersToOpenMessages();

            // ✅ אם שובץ מתנדב – שלח מייל אליו
            if (message.volunteer_id.HasValue)
            {
                var volunteer = await volunteerService.Getbyid(message.volunteer_id.Value);

                if (volunteer != null && !string.IsNullOrEmpty(volunteer.email))
                {
                    await emailService.SendEmailAsync(
                        to: volunteer.email,
                        subject: "שובצת לטיפול בבקשת עזרה",
                        body: $"שלום {volunteer.volunteer_first_name},\n\n" +
                              $"שובצת לטיפול בבקשה חדשה:\n\n" +
                              $"כותרת: {message.title}\n" +
                              $"תיאור: {message.description}\n" +
                              $"כתובת: {message.location}\n\n" +
                              $"אנא אשר הגעה באזור האישי שלך באתר. תודה!"
                    );
                }
            }

            return message;
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] MessageDto value)
        {
            await service.UpDateItem(id, value);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
        }

        // ✅ בקשות לפי המשתמש המחובר (נעזר/מתנדב)
        [Authorize]
        [HttpGet("my-messages")]
        public async Task<List<MessageDto>> GetByUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            var roleClaim = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value?.ToLower();

            if (userIdClaim == null || roleClaim == null)
                return new List<MessageDto>();

            var userId = int.Parse(userIdClaim);
            var allMessages = await service.GetAll();
            var fiveHoursAgo = DateTime.UtcNow.AddHours(-5);

            var filteredMessages = allMessages
                .Where(m =>
                    (roleClaim == "helped" && m.helped_id == userId) ||
                    (roleClaim == "volunteer" &&
                     m.volunteer_id == userId &&
                     (m.confirmArrival == true || m.created_at >= fiveHoursAgo))
                )
                .Select(m =>
                {
                    if (roleClaim == "volunteer" && m.volunteer_id != userId)
                        m.phone = null!;
                    return m;
                })
                .ToList();

            return filteredMessages;
        }
    }
}
 