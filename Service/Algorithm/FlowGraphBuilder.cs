using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository.Entites;

namespace Service.Algorithm
{
    public class FlowGraphBuilder
    {
        // Step 3: Building a flow chart

        //Purpose of the function:
        //Create a flow chart according to your task:
        //Calls ←→ Volunteers, including source and destination
        public FlowGraph BuildGraph(List<Message> messages, List<VolunteerDto> volunteers)
        {
            var graph = new FlowGraph();

            //  Task 3.1: Creating a source node
            graph.AddNode("source");

            //Task 3.4: Creating a sink node
            graph.AddNode("sink");

            //Task 3.2: Create a node for each read and connect to a source with capacity 1
            foreach (var msg in messages) 
            {
                string msgNode = $"m_{msg.message_id}";

                // Create a node for the message
                graph.AddNode(msgNode);

                // Arc from source to message, capacity 1 (each call will only receive one volunteer)
                graph.AddEdge("source", msgNode, int.MaxValue);
            }

            //Task 3.3: Create a node for each volunteer and connect to the sink
            foreach (var vol in volunteers)
            {
                string volNode = $"v_{vol.volunteer_id}";

                // Create a node for the volunteer
                graph.AddNode(volNode);

                // An arc from the volunteer to the sink with unlimited capacity (no limit on the number of requests)
                graph.AddEdge(volNode, "sink", int.MaxValue);
            }
            //Task 3.5: Adding arches calling for suitable volunteers
            foreach (var msg in messages)
            {
                string msgNode = $"m_{msg.message_id}";

                foreach (var vol in volunteers)
                {
                    string volNode = $"v_{vol.volunteer_id}";

                    // Rainbow from the call for volunteers – capacity 1
                    graph.AddEdge(msgNode, volNode, 1);
                }
            }
            return graph;
        }
    }
}
