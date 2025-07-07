using Service.service;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Xunit;

namespace PrijectYedidim.Test.Tests
{
    public class FilterServiceTests
    {
        private readonly FilterService _filterService;

        public FilterServiceTests()
        {
            // מחשב את הנתיב המוחלט לקובץ מתוך תיקיית הבית של הפרויקט
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            var filePath = Path.Combine(projectRoot, "milim_asurot_anik.txt");

            _filterService = new FilterService(filePath);
        }

        [Theory]
        [InlineData("אתה טיפש", true)]
        [InlineData("אני מכוער מאוד", true)]
        [InlineData("זהו משפט תקין לחלוטין", false)]
        [InlineData("שקרן! רמאי! טיפש!", true)]
        public void DetectForbiddenWords(string input, bool expectedHasForbidden)
        {
            var result = _filterService.ContainsForbiddenWords(input);
            Assert.Equal(expectedHasForbidden, result);
        }

        [Fact]
        public void FilterText_ReplacesForbiddenWordsWithStars()
        {
            string input = "הוא גנב ושקר ומכוער";
            string expected = "הוא *** ו*** ו***";

            string result = _filterService.FilterText(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FilterText_DoesNotChangeCleanText()
        {
            string cleanText = "יום נפלא לכולם";
            string result = _filterService.FilterText(cleanText);
            Assert.Equal(cleanText, result);
        }

        [Fact]
        public void CleanDescription_RemovesPunctuationAndNormalizesText()
        {
            string input = "!!! עזרה בבקשה!!!";
            string expected = "עזרה בבקשה";

            string result = _filterService.CleanDescription(input); // ← קריאה לפונקציה מתוך FilterService

            Assert.Equal(expected, result);
        }
    }
}
