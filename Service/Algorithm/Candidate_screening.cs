using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock;
using Porter2Stemmer;
using Repository.Entites;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Service.interfaces;
using Common.Dto;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Service.service;
using System.Reflection.PortableExecutable;



namespace Service.Algorithm
{
  //  Step 2: Screening and matching candidates
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


        public List<Volunteer> GetVolunteersAvailableNow()
        {
            // מנקה את המילישניות – משאיר רק שעה ודקה
            var now = DateTime.Now.TimeOfDay;
            now = new TimeSpan(now.Hours, now.Minutes, 0);

            Console.WriteLine($"⏰ Checking volunteers available at (rounded): {now}");

            var result = _db.Volunteers
                .Where(v => v.IsDeleted == false &&
                            v.start_time.HasValue &&
                            v.end_time.HasValue &&
                            v.start_time.Value <= now &&
                            now <= v.end_time.Value)
                .ToList();

            Console.WriteLine($"🧑‍🤝‍🧑 Found {result.Count} active volunteers available now.");

            foreach (var v in result)
            {
                Console.WriteLine($"✅ {v.volunteer_first_name} {v.volunteer_last_name}, available from {v.start_time} to {v.end_time}");
            }

            return result;
        }

        //public List<Volunteer> GetVolunteersAvailableNow()
        //{
        //    var now = DateTime.Now.TimeOfDay;
        //    Console.WriteLine($"⏰ Checking volunteers available at: {now}");

        //    var result = _db.Volunteers
        //        .Where(v => v.IsDeleted == false &&
        //                    v.start_time.HasValue &&
        //                    v.end_time.HasValue &&
        //                    v.start_time.Value <= now &&
        //                    now <= v.end_time.Value)
        //        .ToList();

        //    Console.WriteLine($"🧑‍🤝‍🧑 Found {result.Count} active volunteers available now.");

        //    foreach (var v in result)
        //    {
        //        Console.WriteLine($"✅ {v.volunteer_first_name} {v.volunteer_last_name}, available from {v.start_time} to {v.end_time}");
        //    }

        //    return result;
        //}


        public async Task<List<VolunteerDto>> FilterVolunteersByDistanceAsync(double helpedLat, double helpedLng)
        {
            Console.WriteLine($"📍 Called FilterVolunteersByDistanceAsync with lat={helpedLat}, lng={helpedLng}");

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
                Console.WriteLine($"➡️ Checking volunteer: {volunteer.volunteer_first_name} {volunteer.volunteer_last_name} (ID={volunteer.volunteer_id}) at ({volunteer.Latitude}, {volunteer.Longitude})");

                var requestUrl =
                    $"https://maps.googleapis.com/maps/api/distancematrix/json" +
                    $"?origins={volunteer.Latitude},{volunteer.Longitude}" +
                    $"&destinations={helpedLat},{helpedLng}" +
                    $"&units=metric&key={apiKey}";

                Console.WriteLine($"🌐 Sending request to: {requestUrl}");

                try
                {
                    var response = await httpClient.GetStringAsync(requestUrl);
                    Console.WriteLine($"📡 Raw API response for volunteer {volunteer.volunteer_id}: {response}");

                    using var doc = JsonDocument.Parse(response);

                    var rows = doc.RootElement.GetProperty("rows");
                    if (rows.GetArrayLength() == 0)
                    {
                        Console.WriteLine("⚠️ No rows returned from Google API.");
                        continue;
                    }

                    var elements = rows[0].GetProperty("elements");
                    if (elements.GetArrayLength() == 0)
                    {
                        Console.WriteLine("⚠️ No elements found in response.");
                        continue;
                    }

                    var distanceElement = elements[0];
                    var status = distanceElement.GetProperty("status").GetString();

                    Console.WriteLine($"📦 Distance matrix status: {status}");

                    if (status != "OK")
                    {
                        Console.WriteLine($"⚠️ Distance matrix status not OK: {status}");
                        continue;
                    }

                    var distanceInMeters = distanceElement
                        .GetProperty("distance")
                        .GetProperty("value")
                        .GetInt32();

                    Console.WriteLine($"📏 Distance to helped: {distanceInMeters} meters");

                    if (distanceInMeters < 10_000)
                    {
                        Console.WriteLine($"✅ Volunteer in range: {volunteer.volunteer_first_name} {volunteer.volunteer_last_name}, phone: {volunteer.tel}, distance: {distanceInMeters} meters");

                        // ⬅️ הוספת תחומי ידע כאן:
                        var knowledge = _db.areas_Of_Knowledges
                            .Where(k => k.volunteer_id == volunteer.volunteer_id)
                            .Select(k => new My_areas_of_knowledge_Dto
                            {
                                describtion = k.describtion
                            }).ToList();  

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
                    else
                    {
                        Console.WriteLine($"🚫 Volunteer {volunteer.volunteer_first_name} is out of range: {distanceInMeters} meters");
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


        // task 2.2 - Get volunteers within 10 km from the given address using Google Maps API

        public List<VolunteerDto> FilterByKnowledge(List<VolunteerDto> filteredVolunteers, Message message)
        {
            var stemmer = new EnglishPorter2Stemmer();

            // Stem words from the message description
            var messageWords = message.description
                .ToLowerInvariant()
                .Split(new[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => stemmer.Stem(w))
                .ToHashSet();

            Console.WriteLine($"📌 Filtering by stemmed keywords from message ID={message.message_id}: [{string.Join(", ", messageWords)}]");

            var result = new List<VolunteerDto>();

            foreach (var v in filteredVolunteers)
            {
                if (v.areas_of_knowledge == null || v.areas_of_knowledge.Count == 0)
                {
                    Console.WriteLine($"🚫 Volunteer {v.volunteer_id} has no knowledge areas.");
                    continue;
                }

                // Stem volunteer areas descriptions
                var volunteerStems = v.areas_of_knowledge
                    .Select(a => stemmer.Stem(a.describtion.ToLowerInvariant()))
                    .ToList();

                Console.WriteLine($"▫ Volunteer {v.volunteer_id} stems: [{string.Join(", ", volunteerStems)}]");

                // Check if any volunteer stem matches any message word stem
                if (volunteerStems.Any(s => messageWords.Contains(s)))
                {
                    Console.WriteLine($"✅ Volunteer {v.volunteer_id} matched by stem overlap.");
                    result.Add(v);
                }
                else
                {
                    Console.WriteLine($"🚫 Volunteer {v.volunteer_id} did NOT match.");
                }
            }

            Console.WriteLine($"🎯 {result.Count} out of {filteredVolunteers.Count} volunteers matched by knowledge.");
            return result;
        }




    }
}