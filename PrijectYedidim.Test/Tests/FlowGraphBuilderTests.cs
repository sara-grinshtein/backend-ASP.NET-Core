using Xunit;
using Service.Algorithm;
using Common.Dto;
using Repository.Entites;
using System.Collections.Generic;
 
public class FlowGraphBuilderTests 
{
    [Fact]
    public void BuildGraph_CreatesExpectedNodesAndEdges()
    {
        // Arrange  
        var messages = new List<Message>
        {
            new Message { message_id = 1, description = "Help with food" },
            new Message { message_id = 2, description = "Need medicine" }
        };

        var volunteers = new List<VolunteerDto> 
        {
            new VolunteerDto { volunteer_id = 10, volunteer_first_name = "Dana" },
            new VolunteerDto { volunteer_id = 20, volunteer_first_name = "Avi" }
        };

        var builder = new FlowGraphBuilder();

        // Act
        var graph = builder.BuildGraph(messages, volunteers);

        // Assert
        Assert.True(graph.Nodes.ContainsKey("source"));
        Assert.True(graph.Nodes.ContainsKey("sink"));

        // 2 messages
        Assert.True(graph.Nodes.ContainsKey("m_1"));
        Assert.True(graph.Nodes.ContainsKey("m_2"));

        // 2 volunteers
        Assert.True(graph.Nodes.ContainsKey("v_10"));
        Assert.True(graph.Nodes.ContainsKey("v_20"));

        // Check edges
        var source = graph.Nodes["source"];
        Assert.Equal(2, source.Edges.Count); // 2 edges to m_1, m_2

        var m1 = graph.Nodes["m_1"];
        Assert.Equal(2, m1.Edges.Count); // 2 volunteers connected

        var vol10 = graph.Nodes["v_10"];
        Assert.Contains(graph.Nodes["sink"], vol10.Edges.Keys);
        Assert.Equal(int.MaxValue, vol10.Edges[graph.Nodes["sink"]]);
    }

    //טסט למצב שאין הודעות בכלל
    [Fact]
    public void BuildGraph_NoMessages_CreatesOnlyVolunteerAndSystemNodes()
    {
        // Arrange
        var messages = new List<Message>(); // no messages
        var volunteers = new List<VolunteerDto>
    {
        new VolunteerDto { volunteer_id = 1, volunteer_first_name = "Test" }
    };

        var builder = new FlowGraphBuilder();

        // Act
        var graph = builder.BuildGraph(messages, volunteers);

        // Assert
        Assert.True(graph.Nodes.ContainsKey("source"));
        Assert.True(graph.Nodes.ContainsKey("sink"));
        Assert.True(graph.Nodes.ContainsKey("v_1"));

        // should be only source, sink, and one volunteer node
        Assert.Equal(3, graph.Nodes.Count);
    }
    //טסט למצב שאין מתנדבים
    [Fact]
    public void BuildGraph_NoVolunteers_CreatesOnlyMessagesAndSystemNodes()
    {
        // Arrange
        var messages = new List<Message>
    {
        new Message { message_id = 1 },
        new Message { message_id = 2 }
    };

        var volunteers = new List<VolunteerDto>(); // no volunteers

        var builder = new FlowGraphBuilder();

        // Act
        var graph = builder.BuildGraph(messages, volunteers);

        // Assert
        Assert.True(graph.Nodes.ContainsKey("m_1"));
        Assert.True(graph.Nodes.ContainsKey("m_2"));
        Assert.Equal(4, graph.Nodes.Count); // source, sink, m_1, m_2

        // Ensure no connections from messages to volunteers
        Assert.All(messages, m =>
        {
            var node = graph.Nodes[$"m_{m.message_id}"];
            Assert.Empty(node.Edges); // לא מחובר לאף מתנדב
        });
    }
    //טסט לוודא שכל מתנדב מחובר רק ל־sink (ולא להודעות בעצמו) 
    [Fact]
    public void BuildGraph_VolunteerEdgesPointOnlyToSink()
    {
        // Arrange
        var messages = new List<Message> { new Message { message_id = 1 } };
        var volunteers = new List<VolunteerDto>
    {
        new VolunteerDto { volunteer_id = 1 },
        new VolunteerDto { volunteer_id = 2 }
    };

        var builder = new FlowGraphBuilder();
        var graph = builder.BuildGraph(messages, volunteers);

        // Assert
        foreach (var v in volunteers)
        {
            var node = graph.Nodes[$"v_{v.volunteer_id}"];
            Assert.Single(node.Edges);
            Assert.Contains(graph.Nodes["sink"], node.Edges.Keys);
        }
    }
    //בונוס: טסט "שלם" להשוואת סך הקשתות
    [Fact]
    public void BuildGraph_TotalEdgesMatchExpectations()
    {
        var messages = new List<Message>
    {
        new Message { message_id = 1 },
        new Message { message_id = 2 }
    };
        var volunteers = new List<VolunteerDto>
    {
        new VolunteerDto { volunteer_id = 1 },
        new VolunteerDto { volunteer_id = 2 },
        new VolunteerDto { volunteer_id = 3 }
    };

        var builder = new FlowGraphBuilder();
        var graph = builder.BuildGraph(messages, volunteers);

        // total expected edges:
        // source -> 2 messages (2 edges)
        // each message -> 3 volunteers (2*3 = 6 edges)
        // 3 volunteers -> sink (3 edges)
        var totalEdges = graph.Nodes.Values.SelectMany(n => n.Edges).Count();
        Assert.Equal(11, totalEdges);
    }

}
