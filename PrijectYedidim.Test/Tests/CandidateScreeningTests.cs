using Common.Dto;
using Service.Algorithm;
using System.Collections.Generic;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Repository.Entites;
using Common.Dto.Common.Dto;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mock;
using Service.interfaces;
using Microsoft.EntityFrameworkCore.InMemory;

namespace PrijectYedidim.Test.Tests
{
    public class CandidateScreeningTests
    {
        public class FakeDistanceService : IDistanceService
        {
            public Task<int?> GetDistanceInMetersAsync(double originLat, double originLng, double destLat, double destLng)
            {
                return Task.FromResult<int?>(5000); // פחות מ־10 ק"מ
            }
        }

        private DataBaseForTest CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataBase>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            return new DataBaseForTest(options);
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => { /* השאר ריק או הוסף פרופילים לפי הצורך */ });
            return config.CreateMapper();
        }

        [Fact]
        public async Task FilterVolunteersByDistanceAndKnowledgeAsync_ShouldReturnCorrectVolunteers()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();

            var volunteer = new Volunteer
            {
                volunteer_id = 1,
                Latitude = 32.0853,
                Longitude = 34.7818,
                IsDeleted = false,
                volunteer_first_name = "דני",
                volunteer_last_name = "כהן",
                email = "dani@example.com",
                tel = "0500000000",
                start_time = TimeSpan.Parse("08:00"),
                end_time = TimeSpan.Parse("16:00"),
                password = "1234"
            };

            var category = new KnowledgeCategory { describtion = "עזרה ראשונה" };

            var knowledge = new My_areas_of_knowledge
            {
                volunteer_id = 1,
                KnowledgeCategory = category
            };

            context.Volunteers.Add(volunteer);
            context.KnowledgeCategories.Add(category);
            context.areas_Of_Knowledges.Add(knowledge);
            await context.SaveChangesAsync();

            var configMock = new ConfigurationBuilder().Build();
            var mapper = CreateMapper();
            var screening = new Candidate_screening(context, configMock, mapper, new FakeDistanceService());

            var message = new Message { message_id = 1, description = "עזרה ראשונה דחופה" };

            // Act
            var result = await screening.FilterVolunteersByDistanceAndKnowledgeAsync(32.0853, 34.7818, message);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].volunteer_id);
        }
    }
}
