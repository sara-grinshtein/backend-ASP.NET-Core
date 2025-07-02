using System;
using System.Collections.Generic;
using System.Linq;
using Repository.interfaces;

namespace Service.Algorithm.Validation
{
    public class Validator
    {
        internal static bool IsValidAssignment(List<(int messageId, int volunteerId)> assignments, out object _)
        {
            throw new NotImplementedException();
        }

        //6.1
        public class AssignmentValidator
        {
            private readonly Icontext _context;

            public AssignmentValidator(Icontext context)
            {
                _context = context;
            }

            public bool IsValidAssignment(List<(int messageId, int volunteerId)> assignments, out string error)
            {
                error = "";

                // 🚫 בדיקה ששום מתנדב שנמחק (IsDeleted) לא שובץ
                if (HasDeletedVolunteers(assignments))
                {
                    error = "שובץ מתנדב שאינו פעיל (IsDeleted = true).";
                    return false;
                }

                // בדיקה מותאמת: מתנדבים מחוץ לשעות הפעילות אבל קרובים עד 19 ק"מ
                if (HasInvalidTimeDistanceAssignments(assignments))
                {
                    error = "שובצו מתנדבים מחוץ לשעות פעילות והם לא קרובים מספיק לאירוע.";
                    return false;
                }

                // בדיקה לחפיפה במרחק בין שיבוצים באותו יום (נסיעה עד 20 דק' - בערך 20 ק"מ)
                if (HasOverlappingAssignments(assignments))
                {
                    error = "יש חפיפת שיבוצים לא חוקית בין קריאות במרחק גדול מדי באותו יום.";
                    return false;
                }

                return true;
            }
            //הפונקציה בודקת אם יש בשיבוצים מתנדב שאינו פעיל או נמחק. אם כן, השיבוץ אינו תקין.
            private bool HasDeletedVolunteers(List<(int messageId, int volunteerId)> assignments)
            {
                var volunteerIds = assignments.Select(a => a.volunteerId).Distinct();
                return _context.Volunteers
                    .Where(v => volunteerIds.Contains(v.volunteer_id))
                    .Any(v => v.IsDeleted);
            }

            private bool HasOverlappingAssignments(List<(int messageId, int volunteerId)> assignments, double maxTravelDistanceKm = 20.0)
            {
                foreach (var (messageId, volunteerId) in assignments)
                {
                    var currentMessage = _context.Messages.FirstOrDefault(m => m.message_id == messageId);
                    if (currentMessage == null) continue;

                    // כל השיבוצים האחרים של אותו מתנדב באותו יום למעט השיבוץ הנוכחי
                    var otherMessages = _context.Messages
                        .Where(m => m.volunteer_id == volunteerId &&
                                    m.date.Date == currentMessage.date.Date &&
                                    m.message_id != messageId)
                        .ToList();

                    foreach (var otherMessage in otherMessages)
                    {
                        // נחשב מרחק בין שתי הקריאות
                        double distanceKm = CalculateDistance(
                            currentMessage.Latitude ?? 0, currentMessage.Longitude ?? 0,
                            otherMessage.Latitude ?? 0, otherMessage.Longitude ?? 0);

                        if (distanceKm > maxTravelDistanceKm)
                        {
                            // אם המרחק גדול מדי, זו חפיפה לא חוקית
                            return true;
                        }
                    }
                }
                return false;
            }

            private bool HasInvalidTimeDistanceAssignments(List<(int messageId, int volunteerId)> assignments, double maxDistanceKm = 19.0)
            {
                var now = DateTime.Now.TimeOfDay;

                foreach (var (messageId, volunteerId) in assignments)
                {
                    var volunteer = _context.Volunteers.FirstOrDefault(v => v.volunteer_id == volunteerId);
                    var message = _context.Messages.FirstOrDefault(m => m.message_id == messageId);

                    if (volunteer == null || message == null)
                        continue;

                    bool isWithinTime = volunteer.start_time.HasValue && volunteer.end_time.HasValue &&
                                        volunteer.start_time.Value <= now && now <= volunteer.end_time.Value;

                    // חישוב מרחק בק"מ בין מתנדב לאירוע
                    double distanceKm = CalculateDistance(
                        volunteer.Latitude ?? 0, volunteer.Longitude ?? 0,
                        message.Latitude ?? 0, message.Longitude ?? 0);

                    // אם המתנדב לא בתוך שעות הפעילות וגם לא קרוב מספיק
                    if (!isWithinTime && distanceKm > maxDistanceKm)
                    {
                        return true;
                    }
                }
                return false;
            }

            private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
            {
                double R = 6371; // רדיוס כדור הארץ בק"מ
                var dLat = ToRadians(lat2 - lat1);
                var dLon = ToRadians(lon2 - lon1);
                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }

            private double ToRadians(double angle)
            {
                return angle * Math.PI / 180.0;
            }
        }
    }
}
