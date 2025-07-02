
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repository.Entites;

namespace Algorithm.Validation
{
    public class DataIntegrityValidator
    {

        //6.4
        private readonly string _logDirectory = "logs";

        public DataIntegrityValidator()
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }

        // ✅ ולידציה למיילים, שמות, זמינות, שדות חובה
        public bool ValidateVolunteerData(List<Volunteer> volunteers)
        {
            bool isValid = true;

            foreach (var v in volunteers)
            {
                if (string.IsNullOrWhiteSpace(v.volunteer_first_name) || string.IsNullOrWhiteSpace(v.email))
                {
                    LogUnmatched("N/A", $"Volunteer {v.volunteer_id} missing name or email");
                    isValid = false;
                    continue;
                }

                if (!v.email.Contains("@") || v.email.Length < 5)
                {
                    LogUnmatched("N/A", $"Invalid email: {v.email} (Volunteer {v.volunteer_id})");
                    isValid = false;
                }

                if (v.volunteer_first_name.Any(char.IsDigit) || IsOnlySymbols(v.volunteer_first_name))
                {
                    LogUnmatched("N/A", $"Invalid name: {v.volunteer_first_name} (Volunteer {v.volunteer_id})");
                    isValid = false;
                }

                if (!v.start_time.HasValue || !v.end_time.HasValue)
                {
                    LogUnmatched("N/A", $"Missing availability times for Volunteer {v.volunteer_id}");
                    isValid = false;
                }
            }

            return isValid;
        }

        // ✅ בדיקת כפילויות בנתוני המתנדבים
        public bool CheckDuplicateEmails(List<Volunteer> volunteers)
        {
            var duplicates = volunteers
                .GroupBy(v => v.email.Trim().ToLower())
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var dup in duplicates)
                LogUnmatched("N/A", $"Duplicate email detected: {dup.Key}");

            return duplicates.Count == 0;
        }

        // ✅ בדיקה שאין קריאה עם תיאור ריק / לא תקני
        public bool ValidateMessages(List<Message> messages)
        {
            bool isValid = true;

            foreach (var m in messages)
            {
                if (string.IsNullOrWhiteSpace(m.description))
                {
                    LogUnmatched(m.message_id.ToString(), "Message title is empty");
                    isValid = false;
                }
                else if (IsOnlySymbols(m.description) || m.description.All(char.IsDigit))
                {
                    LogUnmatched(m.message_id.ToString(), $"Invalid message title: {m.description}");
                    isValid = false;
                }
            }

            return isValid;
        }

        // כלי עזר – טקסט שמכיל רק תווים שאינם אותיות/מספרים
        private bool IsOnlySymbols(string input)
        {
            return input.All(c => !char.IsLetterOrDigit(c));
        }

        // לוג חריגות
        private void LogUnmatched(string entityId, string reason)
        {
            string log = $"[INVALID] {Timestamp()} | ID: {entityId} | {reason}";
            WriteLog("data_validation.log", log);
        }

        private void WriteLog(string fileName, string content)
        {
            string fullPath = Path.Combine(_logDirectory, fileName);
            File.AppendAllText(fullPath, content + Environment.NewLine);
        }

        private string Timestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
