using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Common.Dto.Common.Dto;
using Microsoft.Extensions.Configuration;
using Mock;
using Repository.Entites;
using Service.interfaces;

namespace Service.Algorithm
{
    public class Candidate_screening : ICandidateScreening
    {
        private readonly IMapper _mapper;
        private readonly DataBase _db;
        private readonly IConfiguration _configuration;
        private readonly IDistanceService _distanceService;

        public Candidate_screening(DataBase db, IConfiguration configuration, IMapper mapper, IDistanceService distanceService)
        {
            _db = db;
            _configuration = configuration;
            _mapper = mapper;
            _distanceService = distanceService;
        }

        public List<Volunteer> GetVolunteersAvailableNow()
        {
            return _db.Volunteers
                .Where(v => v.IsDeleted == false && v.Latitude != null && v.Longitude != null)
                .ToList();
        }

        public async Task<List<VolunteerDto>> FilterVolunteersByDistanceAsync(double helpedLat, double helpedLng)
        {
            var allVolunteers = GetVolunteersAvailableNow();
            var nearbyVolunteers = new List<VolunteerDto>();

            foreach (var volunteer in allVolunteers)
            {
                var distanceInMeters = await _distanceService.GetDistanceInMetersAsync(
                    volunteer.Latitude.Value, volunteer.Longitude.Value, helpedLat, helpedLng);

                if (distanceInMeters == null || distanceInMeters >= 10000)
                    continue;

                var knowledge = _db.areas_Of_Knowledges
                    .Where(k => k.volunteer_id == volunteer.volunteer_id)
                    .Select(k => new My_areas_of_knowledge_Dto
                    {
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

            return nearbyVolunteers;
        }

        public List<VolunteerDto> FilterByKnowledge(List<VolunteerDto> volunteers, Message message)
        {
            if (volunteers == null || message == null || string.IsNullOrWhiteSpace(message.description))
            {
                Console.WriteLine("❌ Volunteers or message is null or empty.");
                return new List<VolunteerDto>();
            }

            var matchedVolunteers = new List<VolunteerDto>();

            foreach (var volunteer in volunteers)
            {
                if (volunteer.areas_of_knowledge == null || !volunteer.areas_of_knowledge.Any())
                    continue;

                var knowledgeList = volunteer.areas_of_knowledge
                    .Where(k => !string.IsNullOrWhiteSpace(k.describtion))
                    .Select(k => k.describtion)
                    .ToList();

                double score = GetMatchScore(message.description, knowledgeList);

                if (score > 0)
                {
                    Console.WriteLine($"✅ Volunteer {volunteer.volunteer_id} matched with score {score:P0}");
                    matchedVolunteers.Add(volunteer);
                }
                else
                {
                    Console.WriteLine($"❌ Volunteer {volunteer.volunteer_id} did not match.");
                }
            }

            Console.WriteLine($"🔎 Total matched volunteers: {matchedVolunteers.Count}");
            return matchedVolunteers;
        }

        private double GetMatchScore(string description, List<string> knowledgeAreas)
        {
            if (string.IsNullOrWhiteSpace(description) || knowledgeAreas == null || knowledgeAreas.Count == 0)
                return 0;

            var lowerDescription = description.ToLowerInvariant();
            int matchCount = 0;

            foreach (var area in knowledgeAreas)
            {
                var words = area.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (lowerDescription.Contains(word.ToLower()))
                    {
                        matchCount++;
                        break; // מספיק מילה אחת מכל תחום ידע כדי להחשיב את התחום
                    }
                }
            }

            return (double)matchCount / knowledgeAreas.Count;
        }

        public async Task<List<VolunteerDto>> FilterVolunteersByDistanceAndKnowledgeAsync(
            double helpedLat, double helpedLng, Message message)
        {
            var volunteersNearby = await FilterVolunteersByDistanceAsync(helpedLat, helpedLng);

            if (volunteersNearby == null || !volunteersNearby.Any())
                return new List<VolunteerDto>();

            var results = FilterByKnowledge(volunteersNearby, message);
            Console.WriteLine(results.Count);
            return results;
        }

        public static string CleanDescription(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string noDiacritics = new string(input.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);

            string noPunctuation = Regex.Replace(noDiacritics, @"[\p{P}\p{S}]", " ");
            string cleaned = Regex.Replace(noPunctuation, @"\s+", " ").Trim();

            return cleaned;
        }

        public static string Stem(string word)
        {
            string[] prefixes = { "ה", "ש", "ו", "כ", "ל", "ב", "מ" };
            foreach (var prefix in prefixes)
            {
                if (word.StartsWith(prefix) && word.Length > 3)
                {
                    word = word.Substring(1);
                    break; // מסיר רק קידומת אחת
                }
            }

            string[] suffixes = { "ים", "ות", "ה" };
            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix) && word.Length > 4)
                {
                    word = word.Substring(0, word.Length - suffix.Length);
                    break; // מסיר רק סיומת אחת
                }
            }

            return word;
        }

        private static double CalculateSimilarity(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
                return 0;

            int distance = LevenshteinDistance(s1, s2);
            int maxLen = Math.Max(s1.Length, s2.Length);
            return maxLen == 0 ? 1.0 : 1.0 - (double)distance / maxLen;
        }

        private static int LevenshteinDistance(string s, string t)
        {
            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}
