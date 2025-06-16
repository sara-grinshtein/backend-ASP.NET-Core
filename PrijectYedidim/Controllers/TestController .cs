using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using System.Threading.Tasks;
using Common.Dto;
using Service.service;
using System;

namespace PrijectYedidim.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmbeddingAlgorithmService _embeddingService;
        private readonly IService<VolunteerDto> _volunteerService;

        public TestController(IEmbeddingAlgorithmService embeddingService, IService<VolunteerDto> volunteerService)
        {
            _embeddingService = embeddingService;
            _volunteerService = volunteerService;
        }

        // בדיקה של מתנדבים בטווח 10 ק"מ מקואורדינטות
        [HttpGet("filter-by-distance")]
        public async Task<IActionResult> FilterByDistance([FromQuery] double lat, [FromQuery] double lng)
        {
            if (lat == 0 || lng == 0)
                return BadRequest("יש לספק קואורדינטות תקינות.");

            var volunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);

            if (volunteers == null || volunteers.Count == 0)
            {
                Console.WriteLine("⚠️ No volunteers found in range. Adding test volunteers...");

                for (int i = 1; i <= 3; i++)
                {
                    var dto = new VolunteerDto
                    {
                        volunteer_first_name = $"בדיקה{i}",
                        volunteer_last_name = $"מתנדב{i}",
                        email = $"test{i}_{Guid.NewGuid().ToString().Substring(0, 5)}@example.com",
                        tel = $"05000000{i}",
                        Latitude = lat + 0.0002 * i,
                        Longitude = lng + 0.0002 * i,
                        start_time = TimeSpan.FromHours(0),
                        end_time = TimeSpan.FromHours(23),
                        IsDeleted = false,
                        password = $"Test1234!{i}"
                    };

                    await _volunteerService.AddItem(dto);
                    Console.WriteLine($"🆕 Added volunteer {dto.volunteer_first_name} at location {dto.Latitude}, {dto.Longitude}");
                }

                return Ok("נוספו 3 מתנדבים לבדיקה. נסה שוב.");
            }

            return Ok(volunteers);
        }
    }
}
