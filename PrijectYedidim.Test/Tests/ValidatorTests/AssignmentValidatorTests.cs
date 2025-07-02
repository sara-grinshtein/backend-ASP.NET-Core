using Xunit;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Service.Algorithm.Validation;
using Repository.Entites;
using Mock;
using Tests.TestUtilities;
using static Service.Algorithm.Validation.Validator;

namespace Tests.Validator
{
    /// <summary>
    /// 6.1 
    /// </summary>
    public class AssignmentValidatorTests : IDisposable
    {
        private readonly DataBaseForTest _context;
        private readonly AssignmentValidator _validator;
        private readonly string _dbName;

        public AssignmentValidatorTests()
        {
            _dbName = "Test_" + Guid.NewGuid().ToString("N");

            _context = new DataBaseForTest(_dbName);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _validator = new AssignmentValidator(_context);
        }

        public void Dispose()
        {
            try { _context.Database.EnsureDeleted(); }
            catch { }
            finally { _context.Dispose(); }
        }



        [Fact]
        public void AssignmentValidator_ShouldFail_WhenVolunteerIsDeleted()
        {
            // Arrange
            var volunteer = TestDataBuilder.CreateVolunteer(isDeleted: true);
            var helped = TestDataBuilder.CreateHelped();

            _context.Volunteers.Add(volunteer);
            _context.Helpeds.Add(helped);
            _context.SaveChanges();

            var message = TestDataBuilder.CreateMessage(
                volunteerId: volunteer.volunteer_id,
                helpedId: helped.helped_id
            );

            _context.Messages.Add(message);
            _context.SaveChanges();

            var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

            // Act
            var result = _validator.IsValidAssignment(assignments, out var error);

            // Assert
            Assert.False(result);
            Assert.NotNull(error);
        }


        [Fact]
        public void AssignmentValidator_ShouldPass_WhenVolunteerIsActiveAndValid()
        {
            // Arrange
            var now = DateTime.Now;
            var start = now.AddHours(-1).TimeOfDay;
            var end = now.AddHours(1).TimeOfDay;

            var volunteer = TestDataBuilder.CreateVolunteer(
                isDeleted: false,
                startTime: start,
                endTime: end,
                latitude: 32.1,
                longitude: 34.8
            );

            var helped = TestDataBuilder.CreateHelped(
                latitude: 32.1001,
                longitude: 34.8002
            );

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
            var result = _validator.IsValidAssignment(assignments, out var error);

            Console.WriteLine($"Validation Error: {error}");


            // Assert
            Assert.True(result);
            Assert.True(string.IsNullOrWhiteSpace(error));
        }
        //[Fact]
        //public void AssignmentValidator_ShouldFail_WhenVolunteerTooFar()
        //{
        //    // Arrange
        //    var now = DateTime.Now;
        //    var start = now.AddHours(-1).TimeOfDay;
        //    var end = now.AddHours(1).TimeOfDay;

        //    var volunteer = TestDataBuilder.CreateVolunteer(
        //        isDeleted: false,
        //        startTime: start,
        //        endTime: end,
        //        latitude: 31.0, // מרחק גדול
        //        longitude: 34.0
        //    );

        //    var helped = TestDataBuilder.CreateHelped(
        //        latitude: 32.2,
        //        longitude: 35.0
        //    );

        //    _context.Volunteers.Add(volunteer);
        //    _context.Helpeds.Add(helped);
        //    _context.SaveChanges();

        //    var message = TestDataBuilder.CreateMessage(
        //        volunteerId: volunteer.volunteer_id,
        //        helpedId: helped.helped_id,
        //        date: now,
        //        latitude: 32.2,
        //        longitude: 35.0
        //    );

        //    _context.Messages.Add(message);
        //    _context.SaveChanges();

        //    var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

        //    // Act
        //    var result = _validator.IsValidAssignment(assignments, out var error);

        //    // Assert
        //    Assert.False(result);
        //    Assert.Contains("distance", error, StringComparison.OrdinalIgnoreCase);
        //}

        //[Fact]
        //public void AssignmentValidator_ShouldFail_WhenVolunteerTooFar()
        //{
        //    // Arrange
        //    var now = DateTime.Now;
        //    var start = now.AddHours(-1).TimeOfDay;
        //    var end = now.AddHours(1).TimeOfDay;

        //    var volunteer = TestDataBuilder.CreateVolunteer(
        //        isDeleted: false,
        //        startTime: start,
        //        endTime: end,
        //        latitude: 31.0, // מרחק גדול
        //        longitude: 34.0
        //    );

        //    var helped = TestDataBuilder.CreateHelped(
        //        latitude: 32.2,
        //        longitude: 35.0
        //    );

        //    _context.Volunteers.Add(volunteer);
        //    _context.Helpeds.Add(helped);
        //    _context.SaveChanges();

        //    var message = TestDataBuilder.CreateMessage(
        //        volunteerId: volunteer.volunteer_id,
        //        helpedId: helped.helped_id,
        //        date: now,
        //        latitude: 32.2,
        //        longitude: 35.0
        //    );

        //    _context.Messages.Add(message);
        //    _context.SaveChanges();

        //    var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

        //    // Act
        //    var result = _validator.IsValidAssignment(assignments, out var error);

        //    // Assert
        //    Assert.False(result);
        //    Assert.Contains("distance", error, StringComparison.OrdinalIgnoreCase);
        //}


        //[Fact]
        //public void AssignmentValidator_ShouldFail_WhenVolunteerAlreadyAssignedToAnotherMessage()
        //{
        //    // Arrange
        //    var now = DateTime.Now;
        //    var start = now.AddHours(-1).TimeOfDay;
        //    var end = now.AddHours(1).TimeOfDay;

        //    var volunteer = TestDataBuilder.CreateVolunteer(
        //        isDeleted: false,
        //        startTime: start,
        //        endTime: end,
        //        latitude: 32.1,
        //        longitude: 34.8
        //    );

        //    var helped1 = TestDataBuilder.CreateHelped(latitude: 32.1, longitude: 34.8);
        //    var helped2 = TestDataBuilder.CreateHelped(latitude: 32.1, longitude: 34.8);

        //    _context.Volunteers.Add(volunteer);
        //    _context.Helpeds.AddRange(helped1, helped2);
        //    _context.SaveChanges();

        //    var message1 = TestDataBuilder.CreateMessage(
        //        volunteerId: volunteer.volunteer_id,
        //        helpedId: helped1.helped_id,
        //        date: now,
        //        latitude: 32.1,
        //        longitude: 34.8
        //    );

        //    var message2 = TestDataBuilder.CreateMessage(
        //        volunteerId: null,
        //        helpedId: helped2.helped_id,
        //        date: now,
        //        latitude: 32.1,
        //        longitude: 34.8
        //    );

        //    _context.Messages.AddRange(message1, message2);
        //    _context.SaveChanges();

        //    var assignments = new List<(int, int)> { (message2.message_id, volunteer.volunteer_id) };

        //    // Act
        //    var result = _validator.IsValidAssignment(assignments, out var error);

        //    // Assert
        //    Assert.False(result);
        //    Assert.Contains("already assigned", error, StringComparison.OrdinalIgnoreCase);
        //}


    }
}
