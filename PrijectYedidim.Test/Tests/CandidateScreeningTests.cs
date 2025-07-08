using Common.Dto;
using Service.Algorithm;
using System.Collections.Generic;
using Xunit;
using AutoMapper;
using Moq;
using Microsoft.Extensions.Configuration;
using Mock;
using Repository.Entites;
using Common.Dto.Common.Dto;

namespace PrijectYedidim.Test.Tests
{
    public class CandidateScreeningTests
    {

        [Fact]
        public void FilterByKnowledge_ShouldReturnMatchingVolunteer_WhenDescriptionRoughlyMatchesKnowledge()
        {
            // Arrange
            var dbMock = new Mock<DataBase>();
            var configMock = new Mock<IConfiguration>();
            var mapperMock = new Mock<IMapper>();

            var screening = new Candidate_screening(dbMock.Object, configMock.Object, mapperMock.Object);

            var volunteer = new VolunteerDto
            {
                volunteer_id = 1,
                areas_of_knowledge = new List<My_areas_of_knowledge_Dto>
                {
                    new My_areas_of_knowledge_Dto { describtion = "תכנות" },
                    new My_areas_of_knowledge_Dto { describtion = "מחשבים" }
                }
            };

            var volunteers = new List<VolunteerDto> { volunteer };

            var message = new Message
            {
                message_id = 1,
                description = "אני צריך עזרה בתכנות בסיסי" // טקסט שעדיין אמור להיות דומה ל"תכנות"
            };

            // Act
            var matchedVolunteers = screening.FilterByKnowledge(volunteers, message);

            // Assert
            Assert.True(matchedVolunteers.Any(), "ציפינו למתנדב אחד לפחות שעובר את סף 70% הדמיון");
            Assert.Equal(volunteer.volunteer_id, matchedVolunteers[0].volunteer_id);
        }

        [Fact]
        public void CleanedMessage_ShouldReturnTextWithoutPunctuation()
        {
            var originalText = "!!! עזרה מידית !!!";
            var expected = "עזרה מידית";

            var actual = Candidate_screening.CleanDescription(originalText);

            Assert.Equal(expected, actual);
        }
    }
}
