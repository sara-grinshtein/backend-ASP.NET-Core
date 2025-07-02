using System;
using Repository.Entites;

namespace Tests.TestUtilities
{
    public static class TestDataBuilder
    {
        public static Volunteer CreateVolunteer(
            bool isDeleted = false,
            TimeSpan? startTime = null,
            TimeSpan? endTime = null,
            double latitude = 32.0853,
            double longitude = 34.7818,
            string password = "test123",
            string email = "test@example.com",
            string firstName = "Test",
            string lastName = "Volunteer")
        {
            return new Volunteer
            {
                IsDeleted = isDeleted,
                start_time = startTime ?? TimeSpan.FromHours(9),
                end_time = endTime ?? TimeSpan.FromHours(17),
                Latitude = latitude,
                Longitude = longitude,
                password = password,
                assignment_count = 0,
                email = email,
                volunteer_first_name = firstName,
                volunteer_last_name = lastName
            };
        }

        public static Helped CreateHelped(
            string firstName = "Helped",
            string lastName = "User",
            string email = "helped@example.com",
            string phone = "0500000000",
            string password = "defaultPass123", // חובה במסד
            double latitude = 32.0853,
            double longitude = 34.7818)
        {
            return new Helped
            {
                helped_first_name = firstName,
                helped_last_name = lastName,
                email = email,
                tel = phone,
                password = password, // הוסף שדה סיסמה
                Latitude = latitude,
                Longitude = longitude,
                IsDeleted = false
            };
        }

        public static Message CreateMessage(
    int? volunteerId = null,
   int helpedId = 0,
   DateTime? date = null,
   double latitude = 32.0853,
   double longitude = 34.7818,
   string description = "Test message")
        {
            return new Message
            {
                volunteer_id = volunteerId,
                helped_id = helpedId,
                date = date ?? DateTime.Now,
                Latitude = latitude,
                Longitude = longitude,
                description = description
            };
        }

    }
}
