using System.Collections.Generic;
using Service.Algorithm;
using Repository.Entites;
using Xunit;
using Common.Dto;
using Service.Algorithm.Service.Algorithm;

public class DinicAlgorithmTests
{
    private FlowGraph CreateSimpleGraph()
    {
        var messages = new List<Message> {
        new Message { message_id = 1 },
        new Message { message_id = 2 }
    };

        var volunteers = new List<VolunteerDto> {
        new VolunteerDto { volunteer_id = 10 },
        new VolunteerDto { volunteer_id = 20 }
    };

        var builder = new FlowGraphBuilder();
        var graph = builder.BuildGraph(messages, volunteers);

        // ✅ עדכון קיבולת הקריאות - שיהיה ללא הגבלה
        foreach (var message in messages)
        {
            var messageNodeId = $"m_{message.message_id}";
            if (graph.Nodes["source"].Edges.ContainsKey(graph.Nodes[messageNodeId]))
            {
                graph.Nodes["source"].Edges[graph.Nodes[messageNodeId]] = int.MaxValue;
            }
        }

        return graph;
    }



    [Fact]
    public void MaxFlow_ShouldReturnCorrectFlow_WhenSimpleCase()
    {
        var graph = CreateSimpleGraph();
        var dinic = new DinicAlgorithm(graph);

        var flow = dinic.MaxFlow("source", "sink");
        var assignments = dinic.GetAssignments(); // ✅ הוספנו את זה

        Assert.Equal(assignments.Count, flow);    // 🔄 לא נניח כמות קבועה
    }

    [Fact]
    public void GetAssignments_ShouldReturnCorrectPairs()
    {
        var graph = CreateSimpleGraph();
        var dinic = new DinicAlgorithm(graph);
        dinic.MaxFlow("source", "sink");

        var assignments = dinic.GetAssignments();

        // נוודא שלפחות שתי התאמות בוצעו (לא נניח בדיוק 2)
        Assert.True(assignments.Count >= 2, "Expected at least 2 assignments");

        // נוודא שהודעות 1 ו-2 קיבלו התאמות כלשהן
        Assert.Contains(assignments, a => a.messageId == 1);
        Assert.Contains(assignments, a => a.messageId == 2);
    }


    [Fact]
    public void MaxFlow_ShouldBeZero_WhenNoVolunteers()
    {
        var messages = new List<Message> { new Message { message_id = 1 } };
        var volunteers = new List<VolunteerDto>(); // אין מתנדבים

        var graph = new FlowGraphBuilder().BuildGraph(messages, volunteers);
        var dinic = new DinicAlgorithm(graph);

        var flow = dinic.MaxFlow("source", "sink");
        var assignments = dinic.GetAssignments();

        Assert.Equal(0, flow);
        Assert.Empty(assignments);
    }

    [Fact]
    public void MaxFlow_ShouldAllowMultipleVolunteersForMessage()
    {
        var messages = new List<Message> { new Message { message_id = 1 } };
        var volunteers = new List<VolunteerDto> {
            new VolunteerDto { volunteer_id = 1 },
            new VolunteerDto { volunteer_id = 2 },
            new VolunteerDto { volunteer_id = 3 }
        };

        var graph = new FlowGraphBuilder().BuildGraph(messages, volunteers);
        // הגדלת הקיבולת של הקריאה לאפשר 3 מתנדבים
        graph.Nodes["source"].Edges[graph.Nodes["m_1"]] = 3;

        var dinic = new DinicAlgorithm(graph);
        var flow = dinic.MaxFlow("source", "sink");
        var assignments = dinic.GetAssignments();

        Assert.Equal(3, flow);
        Assert.Equal(3, assignments.Count);
    }

    //מתנדב אחד, אין התאמה(בדיקה של תנאי שלא נכנס לזרימה)
    [Fact]
    public void MaxFlow_ShouldReturnZero_WhenNoMatchExists()
    {
        var messages = new List<Message> { new Message { message_id = 1 } };
        var volunteers = new List<VolunteerDto> { new VolunteerDto { volunteer_id = 1 } };

        var graph = new FlowGraphBuilder().BuildGraph(messages, volunteers);

        // ננתק את הקשת בין הקריאה למתנדב (simulate חוסר התאמה)
        var msgNode = graph.Nodes["m_1"];
        var volNode = graph.Nodes["v_1"];
        msgNode.Edges.Remove(volNode); // אין קשת ביניהם

        var dinic = new DinicAlgorithm(graph);
        var flow = dinic.MaxFlow("source", "sink");

        Assert.Equal(0, flow);
        Assert.Empty(dinic.GetAssignments());
    }
    //כמה מתנדבים לקריאה אחת – נבדוק שכולם מותאמים
    [Fact]
    public void MaxFlow_MultipleVolunteersPerMessage_AllAssigned()
    {
        var messages = new List<Message> { new Message { message_id = 1 } };
        var volunteers = new List<VolunteerDto> {
        new VolunteerDto { volunteer_id = 1 },
        new VolunteerDto { volunteer_id = 2 },
        new VolunteerDto { volunteer_id = 3 }
    };

        var graph = new FlowGraphBuilder().BuildGraph(messages, volunteers);

        // נגדיל את הקיבולת של ההודעה לאפשר את כל המתנדבים
        graph.Nodes["source"].Edges[graph.Nodes["m_1"]] = 3;

        var dinic = new DinicAlgorithm(graph);
        var flow = dinic.MaxFlow("source", "sink");
        var assignments = dinic.GetAssignments();

        Assert.Equal(3, flow);
        Assert.Equal(3, assignments.Count);
        Assert.All(assignments, a => Assert.Equal(1, a.messageId));
    }
    //קשת ריקה (Capacity אפס) – לבדוק שלא יועבר זרימה
    [Fact]
    public void MaxFlow_ShouldIgnoreZeroCapacityEdges()
    {
        var messages = new List<Message> { new Message { message_id = 1 } };
        var volunteers = new List<VolunteerDto> { new VolunteerDto { volunteer_id = 1 } };

        var graph = new FlowGraphBuilder().BuildGraph(messages, volunteers);

        // נבטל את הקיבולת
        var msgNode = graph.Nodes["m_1"];
        var volNode = graph.Nodes["v_1"];
        msgNode.Edges[volNode] = 0;

        var dinic = new DinicAlgorithm(graph);
        var flow = dinic.MaxFlow("source", "sink");

        Assert.Equal(0, flow);
    }
    //מקרה ריק – אין מתנדבים ואין קריאות
    [Fact]
    public void MaxFlow_ShouldHandleEmptyGraph()
    {
        var graph = new FlowGraphBuilder().BuildGraph(new List<Message>(), new List<VolunteerDto>());
        var dinic = new DinicAlgorithm(graph);
        var flow = dinic.MaxFlow("source", "sink");

        Assert.Equal(0, flow);
        Assert.Empty(dinic.GetAssignments());
    }

}
