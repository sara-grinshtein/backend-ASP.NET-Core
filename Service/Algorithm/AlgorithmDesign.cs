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
        public void ApplyAssignments(List<(int messageId, int volunteerId)> assignments)
        {
            foreach (var (messageId, volunteerId) in assignments)
            {
                // Task 5.1: Update Message.volunteer_id
                // Retrieve the message from the database
                var message = _context.Messages.FirstOrDefault(m => m.message_id == messageId);

                // Search for the volunteer in the Volunteers database by ID.
                var volunteer = _context.Volunteers.FirstOrDefault(v => v.volunteer_id == volunteerId);

                if (message != null && volunteer != null)
                {
                    message.volunteer_id = volunteerId;

                    // Task 5.2: Update the volunteer placement count
                    volunteer.assignment_count += 1;  // If this field exists
                }
                // אם אחד מהם לא קיים – לא נבצע כל עדכון, כדי למנוע שגיאות FOREIGN KEY
            }

            // Step 5.3: Save to database
            _context.SaveChanges();
        }
    }
}
