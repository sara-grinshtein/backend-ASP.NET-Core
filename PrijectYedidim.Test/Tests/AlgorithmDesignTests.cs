using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.Algorithm;
using Repository.Entites;
using Mock;
using Tests.TestUtilities;
using Repository.interfaces;
 

namespace Tests.Algorithm
{
    public class AlgorithmDesignTests : IDisposable
    {
        private readonly Icontext _context;
        private readonly AlgorithmDesign _algorithm;
        private readonly string _testDbName;

        public AlgorithmDesignTests()
        {
            _testDbName = "AlgoTest_" + Guid.NewGuid().ToString("N");
            _context = new DataBaseForTest(_testDbName);

            ((DataBaseForTest)_context).Database.EnsureDeleted();
            ((DataBaseForTest)_context).Database.EnsureCreated();

            _algorithm = new AlgorithmDesign(_context);
        }

        public void Dispose()
        {
            try { ((DataBaseForTest)_context).Database.EnsureDeleted(); }
            catch { }
            finally { ((DataBaseForTest)_context).Dispose(); }
        }

        [Fact]
        public void ApplyAssignments_ShouldNotUpdate_WhenMessageNotExists()
        {
            var volunteer = TestDataBuilder.CreateVolunteer();
            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();

            var assignments = new List<(int, int)> { (999, volunteer.volunteer_id) };
            _algorithm.ApplyAssignments(assignments);

            var updatedVolunteer = _context.Volunteers.First(v => v.volunteer_id == volunteer.volunteer_id);
            Assert.Equal(0, updatedVolunteer.assignment_count);
        }

        [Fact]
        public void ApplyAssignments_ShouldNotUpdate_WhenVolunteerNotExists()
        {
            var helped = TestDataBuilder.CreateHelped();
            _context.Helpeds.Add(helped);
            _context.SaveChanges();

            var message = TestDataBuilder.CreateMessage(volunteerId: null, helpedId: helped.helped_id);
            _context.Messages.Add(message);
            _context.SaveChanges();

            var assignments = new List<(int, int)> { (message.message_id, 999) };
            _algorithm.ApplyAssignments(assignments);

            var updatedMessage = _context.Messages.First(m => m.message_id == message.message_id);
            Assert.Null(updatedMessage.volunteer_id);
        }

        [Fact]
        public void ApplyAssignments_ShouldHandleMultipleAssignments()
        {
            var volunteer1 = TestDataBuilder.CreateVolunteer();
            var volunteer2 = TestDataBuilder.CreateVolunteer();
            var helped1 = TestDataBuilder.CreateHelped();
            var helped2 = TestDataBuilder.CreateHelped();

            _context.Volunteers.AddRange(volunteer1, volunteer2);
            _context.Helpeds.AddRange(helped1, helped2);
            _context.SaveChanges();

            var message1 = TestDataBuilder.CreateMessage(volunteerId: null, helpedId: helped1.helped_id);
            var message2 = TestDataBuilder.CreateMessage(volunteerId: null, helpedId: helped2.helped_id);

            _context.Messages.AddRange(message1, message2);
            _context.SaveChanges();

            var assignments = new List<(int, int)>
            {
                (message1.message_id, volunteer1.volunteer_id),
                (message2.message_id, volunteer2.volunteer_id)
            };

            _algorithm.ApplyAssignments(assignments);

            var updatedMessage1 = _context.Messages.First(m => m.message_id == message1.message_id);
            var updatedMessage2 = _context.Messages.First(m => m.message_id == message2.message_id);
            var updatedVolunteer1 = _context.Volunteers.First(v => v.volunteer_id == volunteer1.volunteer_id);
            var updatedVolunteer2 = _context.Volunteers.First(v => v.volunteer_id == volunteer2.volunteer_id);

            Assert.Equal(volunteer1.volunteer_id, updatedMessage1.volunteer_id);
            Assert.Equal(volunteer2.volunteer_id, updatedMessage2.volunteer_id);
            Assert.Equal(1, updatedVolunteer1.assignment_count);
            Assert.Equal(1, updatedVolunteer2.assignment_count);
        }

        [Fact]
        public void ApplyAssignments_ShouldHandleEmptyAssignments()
        {
            var assignments = new List<(int, int)>();
            _algorithm.ApplyAssignments(assignments);
            // Should not throw exception
        }

        [Fact]
        public void ApplyAssignments_ShouldUpdateMessageAndVolunteer_WhenValidAssignment()
        {
            var volunteer = TestDataBuilder.CreateVolunteer();
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

            _algorithm.ApplyAssignments(assignments);

            var updatedMessage = _context.Messages.First(m => m.message_id == message.message_id);
            var updatedVolunteer = _context.Volunteers.First(v => v.volunteer_id == volunteer.volunteer_id);

            Assert.Equal(volunteer.volunteer_id, updatedMessage.volunteer_id);
            Assert.Equal(1, updatedVolunteer.assignment_count);
        }


       
    }
}

