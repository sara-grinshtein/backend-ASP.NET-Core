using AutoMapper;
using Microsoft.Extensions.Configuration;
using Mock;
using Service.Algorithm.Service.Algorithm;
using Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Algorithm
{
    public class ManagerAlgorithm
    {
        private readonly IMapper _mapper;
        private readonly DataBase _db;
        private readonly IConfiguration _configuration;
        private IDistanceService _distanceService;

        public ManagerAlgorithm(DataBase db, IConfiguration configuration, IMapper mapper, IDistanceService distanceService)
        {
            _db = db;
            _configuration = configuration;
            _mapper = mapper;
            _distanceService = distanceService;
        }

        public async Task AssignVolunteersToOpenMessages()
        {
            var fetcher = new DataFetcher(_db, _configuration);
            var candidateScreening = new Candidate_screening(_db, _configuration, _mapper, _distanceService);
            var algorithmDesign = new AlgorithmDesign(_db);

            // שלב 1: שליפת נתונים
            var openMessages = fetcher.GetOpenMessages();
            Console.WriteLine($"📨 נמצאו {openMessages.Count} הודעות פתוחות");

            // שלב 2: סינון מתנדבים לכל הודעה
            var filtered = await Task.WhenAll(
                openMessages.Select(async msg =>
                {
                    Console.WriteLine($"🔎 מתחיל סינון עבור הודעה {msg.message_id}...");
                    var candidates = await candidateScreening.FilterVolunteersByDistanceAndKnowledgeAsync(
                        msg.Latitude ?? 0, msg.Longitude ?? 0, msg);

                    Console.WriteLine($"✅ נמצאו {candidates.Count} מועמדים להודעה {msg.message_id}");
                    return (msg, candidates);
                })
            );

            var allVolunteers = filtered.SelectMany(f => f.candidates).Distinct().ToList();
            Console.WriteLine($"👥 סך כל מתנדבים ייחודיים לאחר סינון: {allVolunteers.Count}");

            // שלב 3: הפעלת הגרף
            var graphBuilder = new FlowGraphBuilder();
            var graph = graphBuilder.BuildGraph(openMessages, allVolunteers);

            var dinic = new DinicAlgorithm(graph);
            dinic.MaxFlow("source", "sink");

            var assignments = dinic.GetAssignments();
            Console.WriteLine($"🔗 מספר שיוכים שהתבצעו: {assignments.Count}");

            algorithmDesign.ApplyAssignments(assignments);
        }


    }
}
