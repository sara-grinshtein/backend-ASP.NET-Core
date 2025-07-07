using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Service.service
{
    public class FilterService
    {
        private readonly HashSet<string> _forbiddenStems;

        // ברירת מחדל: קובץ בשם "forbidden-words.txt"
        private const string DefaultFileName = "forbidden-words.txt";

        public FilterService(string? filePath = null)
        {
            filePath ??= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DefaultFileName);
            Console.WriteLine(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "forbidden-words.txt")));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("קובץ המילים האסורות לא נמצא", filePath);

            _forbiddenStems = File.ReadAllLines(filePath)
                .Select(RemoveDiacritics)
                .Select(RemovePunctuation)
                .Select(w => w.Trim().ToLowerInvariant())
                .Where(w => !string.IsNullOrEmpty(w))
                .Select(Stem)
                .ToHashSet();
        }

        public string CleanDescription(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string noDiacritics = new string(input.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);

            string noPunctuation = Regex.Replace(noDiacritics, @"[\p{P}\p{S}]", " ");
            string cleaned = Regex.Replace(noPunctuation, @"\s+", " ").Trim();

            return cleaned;
        }

        public static string FilterText(string text, string? filePath = null)
        {
            var filterService = new FilterService(filePath);
            return filterService.FilterText(text);
        }

        public bool ContainsForbiddenWords(string text)
        {
            var cleanedWords = SplitTextToWords(RemovePunctuation(RemoveDiacritics(text)));
            return cleanedWords.Any(w => _forbiddenStems.Contains(Stem(w.ToLowerInvariant())));
        }

        public List<string> GetForbiddenWordsInText(string text)
        {
            var cleanedWords = SplitTextToWords(RemovePunctuation(RemoveDiacritics(text)));
            return cleanedWords
                .Where(w => _forbiddenStems.Contains(Stem(w.ToLowerInvariant())))
                .Distinct()
                .ToList();
        }

        public string FilterText(string text)
        {
            var filtered = text;
            var words = Regex.Matches(text, @"\b\S+\b").Cast<Match>().Select(m => m.Value).ToList();

            foreach (var originalWord in words)
            {
                var cleanWord = RemovePunctuation(RemoveDiacritics(originalWord)).ToLowerInvariant();
                var stem = Stem(cleanWord);

                if (_forbiddenStems.Contains(stem))
                {
                    var prefixMatch = Regex.Match(originalWord, @"^(ו|ה|כ|ל|ב|מ|ש)?(.+)$");
                    if (prefixMatch.Success)
                    {
                        var prefix = prefixMatch.Groups[1].Value;
                        var censored = $"{prefix}***";
                        filtered = Regex.Replace(filtered, Regex.Escape(originalWord), censored,
                            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    }
                }
            }

            return filtered;
        }

        private static string RemoveDiacritics(string text) =>
            new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);

        private static string RemovePunctuation(string text) =>
            Regex.Replace(text, @"[^\p{L}\p{N}\s]", string.Empty);

        private static List<string> SplitTextToWords(string text) =>
            Regex.Matches(text, @"\p{L}+", RegexOptions.CultureInvariant)
                .Select(m => m.Value)
                .ToList();

        private static string Stem(string word)
        {
            string[] prefixes = { "ה", "ש", "ו", "כ", "ל", "ב", "מ" };
            foreach (var prefix in prefixes)
            {
                if (word.StartsWith(prefix) && word.Length > 3)
                    word = word.Substring(1);
            }

            return word.Length >= 4 ? word.Substring(0, 4) : word;
        }
    }
}
