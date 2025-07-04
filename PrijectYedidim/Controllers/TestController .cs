﻿using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using System.Threading.Tasks;
using Common.Dto;
using Service.service;
using System;
using Service.Algorithm;
using Repository.Entites;
using System.Collections.Generic;
using Mock;
using System.Linq;

namespace PrijectYedidim.Controllers
{
    [Route("api/test")]
    [ApiController]
    //טסטים עבור סעיפים 1 2 עבור אלגוריתם
    public class TestController : ControllerBase
    {
        private readonly ICandidateScreening _embeddingService;
        private readonly IService<VolunteerDto> _volunteerService;
        private readonly IDataFetcher _dataFetcher;
        private readonly DataBase _db;

        public TestController(ICandidateScreening embeddingService,
                              IDataFetcher dataFetcher,
                              IService<VolunteerDto> volunteerService, DataBase db)
        {
            _embeddingService = embeddingService;
            _volunteerService = volunteerService;
            _dataFetcher = dataFetcher;
            _db = db;
        }

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

        [HttpGet("filter-by-distance-and-knowledge")]
        public async Task<IActionResult> FilterByDistanceAndKnowledge([FromQuery] double lat, [FromQuery] double lng)
        {
            if (lat == 0 || lng == 0)
                return BadRequest("Valid coordinates are required.");

            var helped = _db.Helpeds.FirstOrDefault();
            if (helped == null)
            {
                helped = new Helped
                {
                    helped_first_name = "Test",
                    helped_last_name = "Helped",
                    email = "test_helped@example.com",
                    tel = "0500000000",
                    IsDeleted = false,
                    Latitude = lat,
                    Longitude = lng,
                    password = "Test1234!"
                };
                _db.Helpeds.Add(helped);
                _db.SaveChanges();
            }

            var volunteerIds = new List<int>();

            for (int i = 1; i <= 3; i++)
            {
                var dto = new VolunteerDto
                {
                    volunteer_first_name = $"Test{i}",
                    volunteer_last_name = $"Volunteer{i}",
                    email = $"test{i}_{Guid.NewGuid().ToString().Substring(0, 5)}@example.com",
                    tel = $"05000000{i}",
                    Latitude = lat + 0.0002 * i,
                    Longitude = lng + 0.0002 * i,
                    start_time = TimeSpan.FromHours(0),
                    end_time = TimeSpan.FromHours(23.99),
                    IsDeleted = false,
                    password = $"Test1234!{i}"
                };

                var addedVolunteer = await _volunteerService.AddItem(dto);

                if (addedVolunteer != null && addedVolunteer.volunteer_id != 0)
                {
                    volunteerIds.Add((int)addedVolunteer.volunteer_id);
                    Console.WriteLine($"✅ Volunteer added: {addedVolunteer.volunteer_first_name} {addedVolunteer.volunteer_last_name}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to insert volunteer: {dto.email}");
                }
            }

            var existingKnowledge = _db.areas_Of_Knowledges.FirstOrDefault(a => a.describtion == "Computer");

            foreach (var volunteerId in volunteerIds)
            {
                var alreadyLinked = _db.areas_Of_Knowledges
                    .Any(a => a.volunteer_id == volunteerId && a.describtion == "Computer");

                if (!alreadyLinked)
                {
                    var knowledge = new My_areas_of_knowledge
                    {
                        describtion = "Computer",
                        volunteer_id = volunteerId
                    };

                    _db.areas_Of_Knowledges.Add(knowledge);
                    Console.WriteLine($"🔗 Added knowledge 'Computer' to volunteer ID {volunteerId}");
                }
            }
            _db.SaveChanges();

            var volunteers = await _embeddingService.FilterVolunteersByDistanceAsync(lat, lng);

            if (volunteers.Count == 0)
                return NotFound("❌ No suitable volunteers found.");

            var openMessages = _dataFetcher.GetOpenMessages();
            if (openMessages == null || openMessages.Count == 0)
            {
                var testMessage = new Message
                {
                    helped_id = helped.helped_id,
                    isDone = false,
                    hasResponse = false,
                    description = "Computer problem",
                    Latitude = lat,
                    Longitude = lng
                };

                _db.Messages.Add(testMessage);
                _db.SaveChanges();

                openMessages = _dataFetcher.GetOpenMessages();
            }

            if (openMessages.Count == 0)
                return NotFound("❌ Failed to create test message.");

            var selectedMessage = openMessages.First();
            var filtered = _embeddingService.FilterByKnowledge(volunteers, selectedMessage);

            return Ok(new
            {
                messageTested = selectedMessage.description,
                totalInRange = volunteers.Count,
                matchedByKnowledge = filtered.Count,
                volunteers = filtered
            });
        }

    }
}
