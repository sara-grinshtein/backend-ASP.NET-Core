using Repository.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Service.Algorithm.Validation.Validator;

namespace Service.Algorithm.Validation
{
    public class CapacityValidator
    {
        private readonly Icontext _context;

        public CapacityValidator(Icontext context)
        {
            _context = context;
        }

        /// <summary>
        /// בודק שהכמות הכוללת של השיבוצים שווה לכמות הקריאות הקיימות
        /// </summary>
        public bool IsAssignmentCountValid(List<(int messageId, int volunteerId)> assignments)
        {
            int expectedCount = _context.Messages.Count();
            return assignments.Count == expectedCount;
        }

        /// <summary>
        /// בודק שכל השיבוצים עברו את שלבי הסינון של Validator (שלב 6.1)
        /// </summary>
        public bool AreAllAssignmentsFilteredCorrectly(List<(int messageId, int volunteerId)> assignments)
        {
            // קורא לפונקציה שכבר כתבת ב-Validator
            var assignmentValidator = new AssignmentValidator(_context);
            return assignmentValidator.IsValidAssignment(assignments, out _);
        }
    }
}
