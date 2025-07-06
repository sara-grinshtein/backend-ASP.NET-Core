using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Common.Dto.Common.Dto;
using Microsoft.Extensions.Configuration;
using Mock;
using Porter2Stemmer;
using Repository.Entites;
using Service.interfaces;

namespace Service.Algorithm
{
    // Step 2: Screening and matching candidates
    public class Candidate_screening : ICandidateScreening
    {
        private readonly IMapper _mapper;
        private readonly DataBase _db;
        private readonly IConfiguration _configuration;

        public Candidate_screening(DataBase db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _configuration = configuration;
            _mapper = mapper;
        }

        // החזרת כלל המתנדבים שלא נמחקו (לא תלוי בשעות)
        public List<Volunteer> GetVolunteersAvailableNow()
        {
            return _db.Volunteers
                .Where(v => v.IsDeleted == false && v.Latitude != null && v.Longitude != null)
                .ToList();
        }

        // מביא את כלל המתנדבים בטווח עד 10 ק"מ מכתובת הנעזר
        public async Task<List<VolunteerDto>> FilterVolunteersByDistanceAsync(double helpedLat, double helpedLng)
        {
            Console.WriteLine($"📍 Filtering volunteers near lat={helpedLat}, lng={helpedLng}");
            string apiKey = _configuration["GoogleApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("❌ Missing Google API key.");
                return new List<VolunteerDto>();
            }

            var allVolunteers = GetVolunteersAvailableNow();
            var nearbyVolunteers = new List<VolunteerDto>();
            using var httpClient = new HttpClient();

            foreach (var volunteer in allVolunteers)
            {
                var requestUrl =
                    $"https://maps.googleapis.com/maps/api/distancematrix/json" +
                    $"?origins={volunteer.Latitude},{volunteer.Longitude}" +
                    $"&destinations={helpedLat},{helpedLng}" +
                    $"&units=metric&key={apiKey}";

                try
                {
                    var response = await httpClient.GetStringAsync(requestUrl);
                    using var doc = JsonDocument.Parse(response);

                    var elements = doc.RootElement
                        .GetProperty("rows")[0]
                        .GetProperty("elements")[0];

                    var status = elements.GetProperty("status").GetString();
                    if (status != "OK") continue;

                    var distanceInMeters = elements.GetProperty("distance").GetProperty("value").GetInt32();

                    if (distanceInMeters < 10_000)
                    {
                        var knowledge = _db.areas_Of_Knowledges
                            .Where(k => k.volunteer_id == volunteer.volunteer_id)
                            .Select(k => new My_areas_of_knowledge_Dto {
                                describtion = k.KnowledgeCategory.describtion
                            })
                            .ToList();

                        nearbyVolunteers.Add(new VolunteerDto
                        {
                            volunteer_id = volunteer.volunteer_id,
                            volunteer_first_name = volunteer.volunteer_first_name,
                            volunteer_last_name = volunteer.volunteer_last_name,
                            email = volunteer.email,
                            tel = volunteer.tel,
                            Latitude = volunteer.Latitude,
                            Longitude = volunteer.Longitude,
                            start_time = volunteer.start_time,
                            end_time = volunteer.end_time,
                            IsDeleted = volunteer.IsDeleted,
                            password = volunteer.password,
                            areas_of_knowledge = knowledge
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error: {ex.Message}");
                }
            }

            return nearbyVolunteers;
        }

        // מסנן לפי תחומי ידע (שלב 2.3)
        public List<VolunteerDto> FilterByKnowledge(List<VolunteerDto> volunteers, Message message)
        {
            var stemmer = new EnglishPorter2Stemmer();
            var messageWords = message.description
                .ToLowerInvariant()
                .Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => stemmer.Stem(w))
                .ToHashSet();

            return volunteers.Where(v =>
                v.areas_of_knowledge != null &&
                v.areas_of_knowledge.Select(k => stemmer.Stem(k.describtion.ToLowerInvariant()))
                    .Any(stem => messageWords.Contains(stem))
            ).ToList();
        }

       
    }
}
