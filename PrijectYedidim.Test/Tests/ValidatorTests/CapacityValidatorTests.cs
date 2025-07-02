using System;
using System.Collections.Generic;
using Xunit;
using Mock;
using Repository.Entites;
using Service.Algorithm.Validation;
using Tests.TestUtilities;

namespace Tests.Validator
{
    /// <summary>
    /// 🧪 בדיקות יחידה עבור CapacityValidator (שלב 6.2)
    /// </summary>
    public class CapacityValidatorTests : IDisposable
    {
        private readonly DataBaseForTest _context;
        private readonly CapacityValidator _validator;

        public CapacityValidatorTests()
        {
            _context = new DataBaseForTest("CapacityTest_" + Guid.NewGuid().ToString("N"));
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _validator = new CapacityValidator(_context);
        }

        public void Dispose()
        {
            try
            {
                _context.Database.EnsureDeleted();
            }
            catch { /* במידה והמסד פתוח */ }
            finally
            {
                _context.Dispose();
            }
        }

        [Fact]
        public void IsAssignmentCountValid_ReturnsTrue_WhenCountsMatch()
        {
            // Arrange
            var helped = TestDataBuilder.CreateHelped();
            var volunteer = TestDataBuilder.CreateVolunteer();

            _context.Helpeds.Add(helped);
            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();

            var message = TestDataBuilder.CreateMessage(helpedId: helped.helped_id, volunteerId: volunteer.volunteer_id);

            _context.Messages.Add(message);
            _context.SaveChanges();

            var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

            // Act
            var result = _validator.IsAssignmentCountValid(assignments);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAssignmentCountValid_ReturnsFalse_WhenNoAssignments()
        {
            // Arrange
            var helped = TestDataBuilder.CreateHelped();
            var volunteer = TestDataBuilder.CreateVolunteer();

            _context.Helpeds.Add(helped);
            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();

            var message = TestDataBuilder.CreateMessage(helpedId: helped.helped_id, volunteerId: volunteer.volunteer_id);

            _context.Messages.Add(message);
            _context.SaveChanges();

            var assignments = new List<(int, int)>(); // No assignments

            // Act
            var result = _validator.IsAssignmentCountValid(assignments);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreAllAssignmentsFilteredCorrectly_ReturnsTrue_WhenAllValid()
        {
            // Arrange
            var now = DateTime.Now;

            var volunteer = TestDataBuilder.CreateVolunteer(
                isDeleted: false,
                startTime: now.AddHours(-1).TimeOfDay,
                endTime: now.AddHours(1).TimeOfDay,
                latitude: 32.1,
                longitude: 34.8
            );

            var helped = TestDataBuilder.CreateHelped(latitude: 32.1001, longitude: 34.8002);

            _context.Volunteers.Add(volunteer);
            _context.Helpeds.Add(helped);
            _context.SaveChanges();

            var message = TestDataBuilder.CreateMessage(
                volunteerId: volunteer.volunteer_id,
                helpedId: helped.helped_id,
                date: now,
                latitude: 32.1002,
                longitude: 34.8003
            );

            _context.Messages.Add(message);
            _context.SaveChanges();

            var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

            // Act
            var result = _validator.AreAllAssignmentsFilteredCorrectly(assignments);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AreAllAssignmentsFilteredCorrectly_ReturnsFalse_WhenVolunteerInvalid()
        {
            // Arrange
            var volunteer = TestDataBuilder.CreateVolunteer(isDeleted: true);
            var helped = TestDataBuilder.CreateHelped();

            _context.Volunteers.Add(volunteer);
            _context.Helpeds.Add(helped);
            _context.SaveChanges();

            var message = TestDataBuilder.CreateMessage(volunteerId: volunteer.volunteer_id, helpedId: helped.helped_id);

            _context.Messages.Add(message);
            _context.SaveChanges();

            var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

            // Act
            var result = _validator.AreAllAssignmentsFilteredCorrectly(assignments);

            // Assert
            Assert.False(result);
        }
    }
}
