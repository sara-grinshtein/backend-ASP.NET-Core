using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock;
using Repository.Entites;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Service.interfaces;
using Common.Dto;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Service.service;

namespace Service.Algorithm
{
    public class Candidate_screening : ICandidateScreening
    {
        private readonly IMapper _mapper;

        private readonly DataBase _db;
        private readonly IConfiguration _configuration;
        public Candidate_screening(DataBase db , IConfiguration configuration ,IMapper mapper)
        {
            _db = db;
            _configuration = configuration;
            _mapper = mapper;
        }


        //task 2.1 - Get volunteers available at the current time (filtered from database)
        public List<Volunteer> GetVolunteersAvailableNow()
        {
            var now = DateTime.Now.TimeOfDay;

            return _db.Volunteers
                .Where(v => v.IsDeleted == false &&
                            v.start_time.HasValue &&
                            v.end_time.HasValue &&
                            v.start_time.Value <= now &&
                            now <= v.end_time.Value)
                .ToList();
        }

        // task 2.2 - Get volunteers within 10 km from the given address using Google Maps API

        public async Task<List<VolunteerDto>> FilterVolunteersByDistanceAsync(double helpedLat, double helpedLng)
        {
            string apiKey = _configuration["GoogleApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("❌ Google API key is missing!");
                return new List<VolunteerDto>();
            }

            var availableVolunteers = GetVolunteersAvailableNow()
                .Where(v => v.Latitude != null && v.Longitude != null)
                .ToList();

            Console.WriteLine($"🔍 Total available volunteers with coordinates: {availableVolunteers.Count}");

            var nearbyVolunteers = new List<VolunteerDto>();
            using var httpClient = new HttpClient();

            foreach (var volunteer in availableVolunteers)
            {
                var requestUrl =
                    $"https://maps.googleapis.com/maps/api/distancematrix/json" +
                    $"?origins={volunteer.Latitude},{volunteer.Longitude}" +
                    $"&destinations={helpedLat},{helpedLng}" +
                    $"&units=metric&key={apiKey}";

                try
                {
                    var response = await httpClient.GetStringAsync(requestUrl);
                    Console.WriteLine($"📡 Raw API response for volunteer {volunteer.volunteer_id}: {response}");

                    using var doc = JsonDocument.Parse(response);

                    var rows = doc.RootElement.GetProperty("rows");
                    if (rows.GetArrayLength() == 0) continue;

                    var elements = rows[0].GetProperty("elements");
                    if (elements.GetArrayLength() == 0) continue;

                    var distanceElement = elements[0];
                    if (distanceElement.GetProperty("status").GetString() != "OK") continue;

                    var distanceInMeters = distanceElement
                        .GetProperty("distance")
                        .GetProperty("value")
                        .GetInt32();

                    if (distanceInMeters < 10_000)
                    {
                        Console.WriteLine($"✅ Volunteer in range: {volunteer.volunteer_first_name} {volunteer.volunteer_last_name}, phone: {volunteer.tel}, distance: {distanceInMeters} meters");
                        Console.WriteLine($"Volunteer {volunteer.volunteer_first_name} is {distanceInMeters} meters away.");
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
                            password = volunteer.password // זמני לבדיקה
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error while processing volunteer {volunteer.volunteer_id}: {ex.Message}");
                    continue;
                }
            }

            Console.WriteLine($"📋 Total volunteers found in range: {nearbyVolunteers.Count}");
            return nearbyVolunteers;
        }


        //2.3 checking knowledge areas  
        public List<VolunteerDto> FilterByKnowledge(List<VolunteerDto> filteredVolunteers, Message message)
        {
 
            return filteredVolunteers
                .Where(v => v.areas_of_knowledge != null &&
                            IsKnowledgeMatch(
                                message.description,
                                v.areas_of_knowledge.Select(a => a.describtion).ToList()))
                .ToList();
        }

         // Internal helper – checks overlap between message description and volunteer's knowledge areas
        private bool IsKnowledgeMatch(string messageDescription, List<string> volunteerKnowledge)
        {
            if (string.IsNullOrWhiteSpace(messageDescription) || volunteerKnowledge == null || volunteerKnowledge.Count == 0)
                return false;

            var messageWords = messageDescription
                .ToLower()
                .Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            return volunteerKnowledge.Any(k => messageWords.Contains(k.ToLower()));
        }


    }
}