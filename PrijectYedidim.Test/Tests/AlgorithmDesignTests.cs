using Xunit;
using Service.Algorithm;
using Repository.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Mock;

public class AlgorithmDesignTests : IDisposable
{
    private DataBase _context;

    public AlgorithmDesignTests()
    {
        var options = new DbContextOptionsBuilder<DataBase>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=Test_" + Guid.NewGuid().ToString() + ";Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;")
            .Options;

        _context = new DataBase(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private Helped CreateHelped(string name = "Test Helped", string email = "test@helped.com")
    {
        return new Helped
        {
            helped_first_name = name,
            email = email,
            password = "123"
        };
    }

    private Volunteer CreateVolunteer(string name = "Test Volunteer", string email = "test@volunteer.com")
    {
        return new Volunteer
        {
            volunteer_first_name = name,
            email = email,
            password = "123",
            assignment_count = 0
        };
    }

    private Message CreateMessage(int helpedId, string description = "Test Message")
    {
        return new Message
        {
            description = description,
            date = DateTime.Now,
            helped_id = helpedId,
            isDone = false,
            hasResponse = false
        };
    }

    [Fact]
    public void ApplyAssignments_ShouldAssignVolunteersAndUpdateCount()
    {
        var helped = CreateHelped();
        var volunteer = CreateVolunteer();

        _context.Helpeds.Add(helped);
        _context.Volunteers.Add(volunteer);
        _context.SaveChanges();

        var message = CreateMessage(helped.helped_id);
        _context.Messages.Add(message);
        _context.SaveChanges();

        var service = new AlgorithmDesign(_context);
        var assignments = new List<(int, int)> { (message.message_id, volunteer.volunteer_id) };

        service.ApplyAssignments(assignments);

        var updatedMessage = _context.Messages.First();
        var updatedVolunteer = _context.Volunteers.First();

        Assert.Equal(volunteer.volunteer_id, updatedMessage.volunteer_id);
        Assert.Equal(1, updatedVolunteer.assignment_count);
    }
     
 
    [Fact]
    public void ApplyAssignments_EmptyList_ShouldNotThrow()
    {
        var service = new AlgorithmDesign(_context);
        var assignments = new List<(int, int)>();

        var ex = Record.Exception(() => service.ApplyAssignments(assignments));
        Assert.Null(ex);
    }

    [Fact]
    public void ApplyAssignments_VolunteerNotFound_ShouldNotThrow()
    {
        var helped = CreateHelped();
        _context.Helpeds.Add(helped);
        _context.SaveChanges();

        var message = CreateMessage(helped.helped_id);
        _context.Messages.Add(message);
        _context.SaveChanges();

        var service = new AlgorithmDesign(_context);
        var assignments = new List<(int, int)> { (message.message_id, 999) };

        var ex = Record.Exception(() => service.ApplyAssignments(assignments));

        // לא אמורה להיות חריגה בכלל
        Assert.Null(ex);

        var updatedMessage = _context.Messages.First();

        // volunteer_id אמור להישאר NULL
        Assert.Null(updatedMessage.volunteer_id);
    }
    [Fact]
    public void ApplyAssignments_MessageNotFound_ShouldNotThrow()
    {
        var volunteer = CreateVolunteer();
        _context.Volunteers.Add(volunteer);
        _context.SaveChanges();

        var service = new AlgorithmDesign(_context);
        var assignments = new List<(int, int)> { (999, volunteer.volunteer_id) };

        var ex = Record.Exception(() => service.ApplyAssignments(assignments));
        Assert.Null(ex);

        var updatedVolunteer = _context.Volunteers.First();
        Assert.Equal(0, updatedVolunteer.assignment_count); // 💥 תיקון כאן
    }

    [Fact]
    public void ApplyAssignments_MultipleAssignments_ShouldUpdateAll()
    {
        var helped1 = CreateHelped("Helped 1", "h1@test.com");
        var helped2 = CreateHelped("Helped 2", "h2@test.com");
        var v1 = CreateVolunteer("Alice", "alice@test.com");
        var v2 = CreateVolunteer("Bob", "bob@test.com");

        _context.Helpeds.AddRange(helped1, helped2);
        _context.Volunteers.AddRange(v1, v2);
        _context.SaveChanges();

        var m1 = CreateMessage(helped1.helped_id, "Message 1");
        var m2 = CreateMessage(helped2.helped_id, "Message 2");

        _context.Messages.AddRange(m1, m2);
        _context.SaveChanges();

        var service = new AlgorithmDesign(_context);
        var assignments = new List<(int, int)>
        {
            (m1.message_id, v1.volunteer_id),
            (m2.message_id, v2.volunteer_id)
        };

        service.ApplyAssignments(assignments);

        Assert.Equal(v1.volunteer_id, _context.Messages.First(m => m.message_id == m1.message_id).volunteer_id);
        Assert.Equal(v2.volunteer_id, _context.Messages.First(m => m.message_id == m2.message_id).volunteer_id);
        Assert.Equal(1, _context.Volunteers.First(v => v.volunteer_id == v1.volunteer_id).assignment_count);
        Assert.Equal(1, _context.Volunteers.First(v => v.volunteer_id == v2.volunteer_id).assignment_count);
    }
}
