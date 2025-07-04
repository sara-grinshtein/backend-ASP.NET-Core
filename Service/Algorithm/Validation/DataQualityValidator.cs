﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Repository.Entites;

namespace Algorithm.Validation
{
    /// <summary>
    /// 6.5 Data Quality Validator
    /// </summary>
    public class DataQualityValidator
    {
        // 🧠 משימה 5.1 – בדיקת Regex לשם מתנדב
        public bool IsValidName(string name)
        {
            var pattern = @"^[a-zA-Zא-ת\s]+$";
            return Regex.IsMatch(name ?? "", pattern);
        }

        // 🧠 משימה 5.2 – בדיקת Regex למייל
        public bool IsValidEmail(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]+$";
            return Regex.IsMatch(email ?? "", pattern);
        }

        // 🧠 משימה 5.3 – ניקוי טקסט מתווים חריגים בתיאור
        public string CleanDescription(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // שמירה על אותיות, ספרות, רווחים, סימני פיסוק בסיסיים
            return Regex.Replace(text, @"[^\w\sא-ת.,?!\-()""']", "").Trim();
        }

        // 🧠 משימה 5.4 – בדיקת עומק תיאור (פחות מ־5 מילים → דגל אזהרה)
        public bool IsShallowDescription(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            return wordCount < 5;
        }

        // 🧠 משימה 5.5 – דוגמה לבדיקה גורפת על רשימת מתנדבים
        public List<(int VolunteerId, string Problem)> ValidateVolunteers(List<Volunteer> volunteers)
        {
            var issues = new List<(int, string)>();

            foreach (var v in volunteers)
            {
                if (!IsValidName(v.volunteer_first_name))
                    issues.Add((v.volunteer_id, "Invalid name"));

                if (!IsValidEmail(v.email))
                    issues.Add((v.volunteer_id, "Invalid email"));
            }

            return issues;
        }

        // 🧠 משימה 5.6 – דוגמה לבדיקה גורפת על תיאורים של קריאות
        public List<(int MessageId, string CleanedDescription, bool IsTooShort)> ReviewMessages(List<Message> messages)
        {
            var results = new List<(int, string, bool)>();

            foreach (var msg in messages)
            {
                var cleaned = CleanDescription(msg.description);
                var shallow = IsShallowDescription(cleaned);
                results.Add((msg.message_id, cleaned, shallow));
            }

            return results;
        }
    }
}
