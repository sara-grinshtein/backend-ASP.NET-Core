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

namespace Service.Algorithm
{
    public class Embedding_Algorithm
    {
        private readonly DataBase _db;
        public Embedding_Algorithm(DataBase db)
        {
            _db = db;
        }

        //task 1.1 - Get all unassigned and incomplete help requests
        public List<Message> GetOpenMessages()
        {
            return _db.Messages
                .Where(m => m.volunteer_id == null && m.isDone == false)
                .ToList();
        }

        //task 1.2 - Get all volunteers that are marked as active (not deleted)
        public List<Volunteer> GetAvailableVolunteers()
        {
            return _db.Volunteers
                .Where(v => v.IsDeleted == false)
                .ToList();
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

        public async Task<List<Volunteer>> FilterVolunteersByDistanceAsync(
            double helpedLat,
            double helpedLng)
        {
            // מומלץ להחזיק את המפתח במשתנה-סביבה או בקובץ תצורה.
            const string apiKey = "AIzaSyAsPVKp9pAE4x61AdMqGTooYd4o-X86hwY";

            // מביא רק מתנדבים עם קואורדינטות שמורות (Latitude + Longitude).
            var availableVolunteers = GetVolunteersAvailableNow()
                .Where(v => v.Latitude != null && v.Longitude != null)
                .ToList();

            var nearbyVolunteers = new List<Volunteer>();

            using var httpClient = new HttpClient();

            foreach (var volunteer in availableVolunteers)
            {
                // origins = מיקום המתנדב | destinations = מיקום הנעזר
                var requestUrl =
                    $"https://maps.googleapis.com/maps/api/distancematrix/json" +
                    $"?origins={volunteer.Latitude},{volunteer.Longitude}" +
                    $"&destinations={helpedLat},{helpedLng}" +
                    $"&units=metric&key={apiKey}";

                var response = await httpClient.GetStringAsync(requestUrl);
                using var doc = JsonDocument.Parse(response);

                var distanceInMeters = doc.RootElement
                    .GetProperty("rows")[0]
                    .GetProperty("elements")[0]
                    .GetProperty("distance")
                    .GetProperty("value")
                    .GetInt32();

                if (distanceInMeters < 10_000) // פחות מ-10 ק״מ
                    nearbyVolunteers.Add(volunteer);
            }

            return nearbyVolunteers;
        }

        //    public async Task<List<Volunteer>> FilterVolunteersByDistanceAsync(string helpedLocation)
        //    {
        //        string apiKey = "AIzaSyAsPVKp9pAE4x61AdMqGTooYd4o-X86hwY";

        //        var availableVolunteers = GetVolunteersAvailableNow().Where(
        //            v => !string.IsNullOrEmpty(v.location)).ToList();

        //        var nearbyHelped = new List<Volunteer>();

        //        /*
        //        HttpClient is a class that allows you to send HTTP requests and receive responses – for example, send a request to the Google Maps API.
        //        var httpClient = new HttpClient(); creates a new instance of it.
        //        using var means that the object will be automatically released (Dispose) when the block ends – meaning you don’t have to call httpClient.Dispose() manually.
        //        HttpClient uses internal resources such as network connections.If they are not released, this can cause memory congestion and dead connections.
        //        using ensures proper resource management.
        //         */
        //        using var httpClient = new HttpClient();

        //        foreach (var volunteer in availableVolunteers)
        //        {
        //            // Building a call address for the Distance Matrix API
        //            var requestUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={Uri.EscapeDataString(volunteer.location)}&destinations={Uri.EscapeDataString(helpedLocation)}&key={apiKey}&units=metric";

        //            // Asynchronously calling a URL and receiving a JSON response as a string.
        //            var response = await httpClient.GetStringAsync(requestUrl);

        //            // Now we interpret the contents of the received string  
        //            using var doc = JsonDocument.Parse(response);

        //            /*  Navigation in JSON structure:
        //            rows[0] – the first record (there is only one in this loop).
        //            elements[0] – the first element (single destination).
        //            distance.value – the distance value in meters.
        //            Converts to int and stores in distanceElement. */
        //            var distanceElement = doc.RootElement
        //                .GetProperty("rows")[0]
        //                .GetProperty("elements")[0]
        //                .GetProperty("distance")
        //                .GetProperty("value")
        //                .GetInt32(); // distance in meters

        //            if (distanceElement < 10000) // Less than 10 km
        //            {
        //                nearbyHelped.Add(volunteer);
        //            }
        //        }

        //        return nearbyHelped;
        //    }

        //}
    }
}