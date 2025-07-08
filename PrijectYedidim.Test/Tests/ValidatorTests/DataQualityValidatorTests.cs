//using Xunit;
//using System;
//using System.Collections.Generic;
//using Algorithm.Validation;
//using Repository.Entites;

//namespace ProjectYedidim.Test.Validation
//{
//    public class DataQualityValidatorTests
//    {
//        private readonly DataQualityValidator _validator = new();

//        // 5.1 – בדיקת שם
//        [Theory]
//        [InlineData("יוסי כהן", true)]
//        [InlineData("John Doe", true)]
//        [InlineData("1234", false)]
//        [InlineData("!!!", false)]
//        [InlineData(null, false)]
//        [InlineData("", false)]
//        public void Test_IsValidName(string input, bool expected)
//        {
//            var result = _validator.IsValidName(input);
//            Assert.Equal(expected, result);
//        }

//        // 5.2 – בדיקת מייל
//        [Theory]
//        [InlineData("test@example.com", true)]
//        [InlineData("user.name@domain.co.il", true)]
//        [InlineData("invalidemail", false)]
//        [InlineData("user@.com", false)]
//        [InlineData("", false)]
//        [InlineData(null, false)]
//        public void Test_IsValidEmail(string input, bool expected)
//        {
//            var result = _validator.IsValidEmail(input);
//            Assert.Equal(expected, result);
//        }

//        // 5.3 – ניקוי תווים חריגים
//        [Fact]
//        public void Test_CleanDescription_RemovesInvalidCharacters()
//        {
//            var input = "שלום! עזרה בבקשה!!! @#$%^&*";
//            var expected = "שלום! עזרה בבקשה!!!"; // ← מתוקן בהתאם לפונקציה שלך
//            var result = _validator.CleanDescription(input);
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void Test_CleanDescription_NullOrEmpty_ReturnsEmpty()
//        {
//            Assert.Equal(string.Empty, _validator.CleanDescription(null));
//            Assert.Equal(string.Empty, _validator.CleanDescription(""));
//        }

//        // 5.4 – עומק תיאור
//        [Theory]
//        [InlineData("אני צריך עזרה מידית", true)] // 4 מילים
//        [InlineData("עזרה דחופה במקום זה", true)] // 4
//        [InlineData("עזרה דחופה עכשיו תודה רבה", false)] // 5
//        [InlineData("יש לי בעיה חמורה מאוד כאן", false)] // 6
//        [InlineData(null, true)]
//        [InlineData("", true)]
//        public void Test_IsShallowDescription(string text, bool expected)
//        {
//            var result = _validator.IsShallowDescription(text);
//            Assert.Equal(expected, result);
//        }

//        // 5.5 – בדיקת מתנדבים
//        [Fact]
//        public void Test_ValidateVolunteers_Invalids_ReturnsIssues()
//        {
//            var list = new List<Volunteer>
//            {
//                new Volunteer { volunteer_id = 1, volunteer_first_name = "!!!", email = "email" },
//                new Volunteer { volunteer_id = 2, volunteer_first_name = "Valid", email = "valid@email.com" },
//                new Volunteer { volunteer_id = 3, volunteer_first_name = "1234", email = "also@bad" }
//            };

//            var issues = _validator.ValidateVolunteers(list);

//            Assert.Contains(issues, i => i.VolunteerId == 1 && i.Problem.Contains("Invalid"));
//            Assert.Contains(issues, i => i.VolunteerId == 3 && i.Problem.Contains("Invalid"));
//            Assert.DoesNotContain(issues, i => i.VolunteerId == 2);
//        }

//        [Fact]
//        public void Test_ValidateVolunteers_EmptyList_ReturnsEmpty()
//        {
//            var result = _validator.ValidateVolunteers(new List<Volunteer>());
//            Assert.Empty(result);
//        }

//        // 5.6 – בדיקת תיאורים
//        [Fact]
//        public void Test_ReviewMessages_ReturnsCleanedAndShallowFlags()
//        {
//            var messages = new List<Message>
//            {
//                new Message { message_id = 1, description = "!!! עזרה מידית !!!" },
//                new Message { message_id = 2, description = "יש בעיה חמורה שדורשת פתרון" },
//                new Message { message_id = 3, description = "" },
//            };

//            var result = _validator.ReviewMessages(messages);

//            Assert.Equal(3, result.Count);

//            Assert.Equal("!!! עזרה מידית !!!", result[0].CleanedDescription); // ← לא מוסר סימני קריאה
//            Assert.True(result[0].IsTooShort);

//            Assert.False(result[1].IsTooShort);
//            Assert.Equal("יש בעיה חמורה שדורשת פתרון", result[1].CleanedDescription);

//            Assert.Equal(string.Empty, result[2].CleanedDescription);
//            Assert.True(result[2].IsTooShort);
//        }

//        [Fact]
//        public void Test_ReviewMessages_NullDescription_HandledGracefully()
//        {
//            var messages = new List<Message>
//            {
//                new Message { message_id = 1, description = null }
//            };

//            var result = _validator.ReviewMessages(messages);

//            Assert.Single(result);
//            Assert.Equal(string.Empty, result[0].CleanedDescription);
//            Assert.True(result[0].IsTooShort);
//        }
//    }
//}

using Xunit;
using System;
using System.Collections.Generic;
using Algorithm.Validation;
using Repository.Entites;
using Service.service; // ← חשוב

namespace ProjectYedidim.Test.Validation
{
    public class DataQualityValidatorTests
    {
        private readonly DataQualityValidator _validator = new();
        private readonly FilterService _filterService;

        public DataQualityValidatorTests()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            var filePath = Path.Combine(projectRoot, "milim_asurot_anik.txt");
            _filterService = new FilterService(filePath);
        }

        // 5.1 – בדיקת שם
        [Theory]
        [InlineData("יוסי כהן", true)]
        [InlineData("John Doe", true)]
        [InlineData("1234", false)]
        [InlineData("!!!", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void Test_IsValidName(string input, bool expected)
        {
            var result = _validator.IsValidName(input);
            Assert.Equal(expected, result);
        }

        // 5.2 – בדיקת מייל
        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("user.name@domain.co.il", true)]
        [InlineData("invalidemail", false)]
        [InlineData("user@.com", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Test_IsValidEmail(string input, bool expected)
        {
            var result = _validator.IsValidEmail(input);
            Assert.Equal(expected, result);
        }

        // 5.3 – ניקוי תווים חריגים
        [Fact]
        public void Test_CleanDescription_RemovesInvalidCharacters()
        {
            var input = "שלום! עזרה בבקשה!!! @#$%^&*";
            var expected = "שלום עזרה בבקשה";

            var result = _filterService.CleanDescription(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_CleanDescription_NullOrEmpty_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, _filterService.CleanDescription(null));
            Assert.Equal(string.Empty, _filterService.CleanDescription(""));
        }

        // 5.4 – עומק תיאור
        [Theory]
        [InlineData("אני צריך עזרה מידית", true)]
        [InlineData("עזרה דחופה במקום זה", true)]
        [InlineData("עזרה דחופה עכשיו תודה רבה", false)]
        [InlineData("יש לי בעיה חמורה מאוד כאן", false)]
        [InlineData(null, true)]
        [InlineData("", true)]
        public void Test_IsShallowDescription(string text, bool expected)
        {
            var result = _validator.IsShallowDescription(text);
            Assert.Equal(expected, result);
        }

        // 5.5 – בדיקת מתנדבים
        [Fact]
        public void Test_ValidateVolunteers_Invalids_ReturnsIssues()
        {
            var list = new List<Volunteer>
            {
                new Volunteer { volunteer_id = 1, volunteer_first_name = "!!!", email = "email" },
                new Volunteer { volunteer_id = 2, volunteer_first_name = "Valid", email = "valid@email.com" },
                new Volunteer { volunteer_id = 3, volunteer_first_name = "1234", email = "also@bad" }
            };

            var issues = _validator.ValidateVolunteers(list);

            Assert.Contains(issues, i => i.VolunteerId == 1 && i.Problem.Contains("Invalid"));
            Assert.Contains(issues, i => i.VolunteerId == 3 && i.Problem.Contains("Invalid"));
            Assert.DoesNotContain(issues, i => i.VolunteerId == 2);
        }

        [Fact]
        public void Test_ValidateVolunteers_EmptyList_ReturnsEmpty()
        {
            var result = _validator.ValidateVolunteers(new List<Volunteer>());
            Assert.Empty(result);
        }

        // 5.6 – בדיקת תיאורים
        [Fact]
        public void Test_ReviewMessages_ReturnsCleanedAndShallowFlags()
        {
            var messages = new List<Message>
            {
                new Message { message_id = 1, description = "!!! עזרה מידית !!!" },
                new Message { message_id = 2, description = "יש בעיה חמורה שדורשת פתרון" },
                new Message { message_id = 3, description = "" },
            };

            var result = _validator.ReviewMessages(messages);

            Assert.Equal(3, result.Count);

            Assert.Equal("עזרה מידית", result[0].CleanedDescription); // תיקון כאן
            Assert.True(result[0].IsTooShort);

            Assert.False(result[1].IsTooShort);
            Assert.Equal("יש בעיה חמורה שדורשת פתרון", result[1].CleanedDescription);

            Assert.Equal(string.Empty, result[2].CleanedDescription);
            Assert.True(result[2].IsTooShort);
        }

        [Fact]
        public void Test_ReviewMessages_NullDescription_HandledGracefully()
        {
            var messages = new List<Message>
            {
                new Message { message_id = 1, description = null }
            };

            var result = _validator.ReviewMessages(messages);

            Assert.Single(result);
            Assert.Equal(string.Empty, result[0].CleanedDescription);
            Assert.True(result[0].IsTooShort);
        }
    }
}

