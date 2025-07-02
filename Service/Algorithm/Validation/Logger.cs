using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Entites;

namespace Service.Algorithm.Logging
{
    public class AssignmentLogger
    {
        public static void LogMatches(
            List<(int messageId, int volunteerId)> assignments,
            List<Message> messages,
            List<Volunteer> volunteers)
        {
            Console.WriteLine("📝 Assignment Log:");
            foreach (var (messageId, volunteerId) in assignments)
            {
                var msg = messages.FirstOrDefault(m => m.message_id == messageId);
                var vol = volunteers.FirstOrDefault(v => v.volunteer_id == volunteerId);
                Console.WriteLine($"Matched Message ID: {messageId} (Helped ID: {msg?.helped_id}) -> Volunteer ID: {volunteerId} ({vol?.volunteer_first_name})");
            }
        }

        public static void LogUnassigned(List<Message> allMessages, List<(int messageId, int volunteerId)> assignments)
        {
            var assignedIds = assignments.Select(a => a.messageId).ToHashSet();
            var unassigned = allMessages.Where(m => !assignedIds.Contains(m.message_id));

            Console.WriteLine("\n⚠️ Unassigned Calls Log:");
            foreach (var msg in unassigned)
            {
                Console.WriteLine($"Call {msg.message_id}: NOT ASSIGNED (Reason: No eligible volunteer found)");
            }
        }

        public static void LogTimestamp(string stage, DateTime start, DateTime? previous = null)
        {
            var now = DateTime.Now;
            string duration = previous.HasValue ? $"(took {(now - previous.Value).TotalMilliseconds} ms)" : "";
            Console.WriteLine($"📅 [{now:HH:mm:ss.fff}] {stage} completed {duration}");
        }

        public static void LogSummary(List<Volunteer> allVolunteers, List<(int messageId, int volunteerId)> assignments, List<Message> allMessages)
        {
            var usedVolunteers = assignments.Select(a => a.volunteerId).Distinct().Count();
            var totalVolunteers = allVolunteers.Count;
            var totalCalls = allMessages.Count;
            var assignedCalls = assignments.Count;

            Console.WriteLine("\n📈 Summary:");
            Console.WriteLine($"Volunteers used: {usedVolunteers}/{totalVolunteers}");
            Console.WriteLine($"Calls assigned:  {assignedCalls}/{totalCalls}");
        }
    }
}
