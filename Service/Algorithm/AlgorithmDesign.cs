using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Algorithm
{
    // Update database -5
    public class AlgorithmDesign
    {
        private readonly Icontext _context;

        public AlgorithmDesign(Icontext context)
        {
            _context = context;
        }

        // Each pair (messageId, volunteerId) says: "Assign this volunteer to this call."
        //public void ApplyAssignments(List<(int messageId, int volunteerId)> assignments)
        //{
        //    foreach (var (messageId, volunteerId) in assignments)
        //    {
        //        // Task 5.1: Update Message.volunteer_id
        //        // Retrieve the message from the database
        //        var message = _context.Messages.FirstOrDefault(m => m.message_id == messageId);

        //        // Search for the volunteer in the Volunteers database by ID.
        //        var volunteer = _context.Volunteers.FirstOrDefault(v => v.volunteer_id == volunteerId);

        //        if (message != null && volunteer != null)
        //        {
        //            message.volunteer_id = volunteerId;

        //            // Task 5.2: Update the volunteer placement count
        //            volunteer.assignment_count += 1;  // If this field exists
        //        }
        //        // אם אחד מהם לא קיים – לא נבצע כל עדכון, כדי למנוע שגיאות FOREIGN KEY
        //    }

        //    // Step 5.3: Save to database
        //    _context.SaveChanges();
        //}

        public void ApplyAssignments(List<(int messageId, int volunteerId)> assignments)
        {
            // קיבוץ כל השיוכים לפי הודעה
            var grouped = assignments.GroupBy(a => a.messageId);

            foreach (var group in grouped)
            {
                // שליפת ההודעה המקורית מה־DB
                var originalMessage = _context.Messages.FirstOrDefault(m => m.message_id == group.Key);
                if (originalMessage == null) continue;

                var volunteers = group.Select(g => g.volunteerId).ToList();

                for (int i = 0; i < volunteers.Count; i++)
                {
                    var volunteerId = volunteers[i];
                    var volunteer = _context.Volunteers.FirstOrDefault(v => v.volunteer_id == volunteerId);
                    if (volunteer == null) continue;

                    Message messageToAssign;

                    if (i == 0)
                    {
                        // שימוש בהודעה המקורית לשיבוץ הראשון
                        messageToAssign = originalMessage;
                    }
                    else
                    {
                        // יצירת עותק חדש מההודעה המקורית
                        messageToAssign = new Message
                        {
                            helped_id = originalMessage.helped_id,
                            description = originalMessage.description,
                            Latitude = originalMessage.Latitude,
                            Longitude = originalMessage.Longitude,
                            isDone = originalMessage.isDone,
                            ConfirmArrival= originalMessage.ConfirmArrival
                            // הוספה לשימור שדות נוספים בעתיד אם יש
                        };
                        _context.Messages.Add(messageToAssign);
                    }

                    // Task 5.1: עדכון שדה volunteer_id בהודעה
                    messageToAssign.volunteer_id = volunteerId;

                    // Task 5.2: עדכון מספר השיבוצים של המתנדב
                    volunteer.assignment_count += 1;  // אם השדה הזה קיים
                }
            }

            // שלב 5.3: שמירת כל העדכונים בבסיס הנתונים
            _context.SaveChanges();
        }

    }
}
