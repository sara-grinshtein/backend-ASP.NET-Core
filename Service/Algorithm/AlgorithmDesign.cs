using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Entites;
using Microsoft.EntityFrameworkCore;
using Repository.interfaces;

namespace Service.Algorithm
{
    public class AlgorithmDesign
    {
        private readonly  Icontext _context;

        public AlgorithmDesign(Icontext context)
        {
            _context = context;
        }

        public void ApplyAssignments(List<(int messageId, int volunteerId)> assignments)
        {
            foreach (var (messageId, volunteerId) in assignments)
            {
                // שליפת ההודעה מהמסד
                var message = _context.Messages.FirstOrDefault(m => m.message_id == messageId);
                if (message != null)
                {
                    message.volunteer_id = volunteerId;
                }

                // עדכון מונה שיבוצים למתנדב
                var volunteer = _context.Volunteers.FirstOrDefault(v => v.volunteer_id == volunteerId);
                if (volunteer != null)
                {
                    volunteer.assignment_count += 1; // אם השדה הזה קיים
                }
            }

            // שלב 5.3: שמירה למסד הנתונים
            _context.SaveChanges();
        }
    }
}
