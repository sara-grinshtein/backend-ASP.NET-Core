//using Microsoft.AspNetCore.Mvc;
//using Service.interfaces;
//using System.Threading.Tasks;
//using Common.Dto;
//using Service.service;
//using System;
//using Service.Algorithm;
//using Repository.Entites;
//using System.Collections.Generic;

//namespace PrijectYedidim.Controllers
//{
//    [Route("api/test")]
//    [ApiController]
//    public class TestController : ControllerBase
//    {
//        private readonly ICandidateScreening _embeddingService;
//        private readonly IService<VolunteerDto> _volunteerService;
//        private readonly IDataFetcher _dataFetcher;

//        public TestController(ICandidateScreening embeddingService,
//                              IDataFetcher dataFetcher,
//                              IService<VolunteerDto> volunteerService)
//        {
//            _embeddingService = embeddingService;
//            _volunteerService = volunteerService;
//            _dataFetcher = dataFetcher;
//        }

//        // שלב 2.2 – סינון לפי מרחק
//        [HttpGet("filter-by-distance")]
//        public async Task<IActionResult> FilterByDistance([FromQuery] double lat, [FromQuery] double lng)
//        {
//            if (lat == 0 || lng == 0)
//                return BadRequest("יש לספק קואורדינטות תקינות.");

//            var volunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);

//            if (volunteers == null || volunteers.Count == 0)
//            {
//                Console.WriteLine("⚠️ No volunteers found in range. Adding test volunteers...");

//                for (int i = 1; i <= 3; i++)
//                {
//                    var dto = new VolunteerDto
//                    {
//                        volunteer_first_name = $"בדיקה{i}",
//                        volunteer_last_name = $"מתנדב{i}",
//                        email = $"test{i}_{Guid.NewGuid().ToString().Substring(0, 5)}@example.com",
//                        tel = $"05000000{i}",
//                        Latitude = lat + 0.0002 * i,
//                        Longitude = lng + 0.0002 * i,
//                        start_time = TimeSpan.FromHours(0),
//                        end_time = TimeSpan.FromHours(23),
//                        IsDeleted = false,
//                        password = $"Test1234!{i}"
//                    };

//                    await _volunteerService.AddItem(dto);
//                    Console.WriteLine($"🆕 Added volunteer {dto.volunteer_first_name} at location {dto.Latitude}, {dto.Longitude}");
//                }

//                return Ok("נוספו 3 מתנדבים לבדיקה. נסה שוב.");
//            }

//            return Ok(volunteers);
//        }

//        // שלב 2.2 + 2.3 – סינון לפי מרחק + תחומי ידע
//        [HttpGet("filter-by-distance-and-knowledge")]
//        public async Task<IActionResult> FilterByDistanceAndKnowledge([FromQuery] double lat, [FromQuery] double lng)
//        {
//            if (lat == 0 || lng == 0)
//                return BadRequest("יש לספק קואורדינטות תקינות.");

//            // שלב 2.2 – סינון לפי מרחק
//            var volunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);
//            if (volunteers == null || volunteers.Count == 0)
//                return NotFound("❌ לא נמצאו מתנדבים בטווח.");

//            // שלב 1.1 – קריאה פתוחה לבדיקה
//            var openMessages = _dataFetcher.GetOpenMessages();
//            if (openMessages == null || openMessages.Count == 0)
//                return NotFound("❌ אין קריאות פתוחות במערכת.");

//            var testMessage = openMessages.First(); // ניקח את הקריאה הראשונה לבדיקה

//            // שלב 2.3 – סינון לפי תחומי ידע
//            var filtered = _embeddingService.FilterByKnowledge(volunteers, testMessage);

//            return Ok(new
//            {
//                messageTested = testMessage.description,
//                totalInRange = volunteers.Count,
//                matchedByKnowledge = filtered.Count,
//                volunteers = filtered
//            });
//        }
//    }
//}
using Mock;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using System.Threading.Tasks;
using Common.Dto;
using Service.service;
using System;
using Service.Algorithm;
using Repository.Entites;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PrijectYedidim.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICandidateScreening _embeddingService;
        private readonly IService<VolunteerDto> _volunteerService;
        private readonly IDataFetcher _dataFetcher;
        private readonly DataBase _db;

        public TestController(
            ICandidateScreening embeddingService,
            IDataFetcher dataFetcher,
            IService<VolunteerDto> volunteerService,
            DataBase db)
        {
            _embeddingService = embeddingService;
            _volunteerService = volunteerService;
            _dataFetcher = dataFetcher;
            _db = db;
        }

        // שלב 2.2 - רק לפי מרחק
        [HttpGet("filter-by-distance")]
        public async Task<IActionResult> FilterByDistance([FromQuery] double lat, [FromQuery] double lng)
        {
            if (lat == 0 || lng == 0)
                return BadRequest("יש לספק קואורדינטות תקינות.");

            var volunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);

            return Ok(volunteers);
        }

        [HttpGet("filter-by-distance-and-knowledge")]
        public async Task<IActionResult> FilterByDistanceAndKnowledge([FromQuery] double lat, [FromQuery] double lng)
        {
            if (lat == 0 || lng == 0)
                return BadRequest("יש לספק קואורדינטות תקינות.");

            // שלב 2.2 – סינון לפי מרחק
            var volunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);
            if (volunteers == null || volunteers.Count == 0)
                return NotFound("❌ לא נמצאו מתנדבים בטווח.");

            // שלב 1.1 – קריאה פתוחה
            var openMessages = _dataFetcher.GetOpenMessages();
            Message testMessage;

            if (openMessages == null || openMessages.Count == 0)
            {
                // אם אין helped – צור אחד
                var helped = await _db.Helpeds.FirstOrDefaultAsync();
                if (helped == null)
                {
                    helped = new Helped
                    {
                        helped_first_name = "בדיקה",
                        helped_last_name = "מערכת",
                        tel = "0501234567",
                        password = "Test1234!"
                    };
                    _db.Helpeds.Add(helped);
                    await _db.SaveChangesAsync();
                }

                // צור הודעת בדיקה
                testMessage = new Message
                {
                    description = "neeed help in computers ",
                    helped_id = helped.helped_id,
                    isDone = false,
                    hasResponse = false,
                    Latitude = lat,
                    Longitude = lng
                };

                _db.Messages.Add(testMessage);
                await _db.SaveChangesAsync();
            }
            else
            {
                testMessage = openMessages.First();
            }

            // שלב הוספת מתנדב עם תחום ידע תואם אם אין קיים
            var existing = _db.Volunteers
                .Include(v => v.areas_of_knowledge)
                .FirstOrDefault(v => v.volunteer_first_name == "בדיקה ידע" && v.Latitude == lat);

            if (existing == null)
            {
                var knowledge = await _db.areas_Of_Knowledges
                    .FirstOrDefaultAsync(k => k.describtion == "computers");

                if (knowledge == null)
                {
                    knowledge = new My_areas_of_knowledge { describtion = "computers" };
                    _db.areas_Of_Knowledges.Add(knowledge);
                    await _db.SaveChangesAsync();
                }

                var volunteer = new Volunteer
                {
                    volunteer_first_name = "בדיקה ידע",
                    volunteer_last_name = "מתנדב",
                    email = $"test_{Guid.NewGuid().ToString().Substring(0, 5)}@example.com",
                    tel = "0509999999",
                    Latitude = lat,
                    Longitude = lng,
                    start_time = TimeSpan.FromHours(0),
                    end_time = TimeSpan.FromHours(23),
                    IsDeleted = false,
                    password = "Test1234!",
                    areas_of_knowledge = new List<My_areas_of_knowledge> { knowledge }
                };

                _db.Volunteers.Add(volunteer);
                await _db.SaveChangesAsync();
            }

            // שלב 2.3 – סינון לפי תחומי ידע
            var updatedVolunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);
            var filtered = _embeddingService.FilterByKnowledge(updatedVolunteers, testMessage);

            return Ok(new
            {
                messageTested = testMessage.description,
                totalInRange = updatedVolunteers.Count,
                matchedByKnowledge = filtered.Count,
                volunteers = filtered
            });
        }

    }
}






