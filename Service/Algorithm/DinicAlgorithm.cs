using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Repository.Entites;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Algorithm
{
    namespace Service.Algorithm
    {
        public class DinicAlgorithm
        {
            //graph: The graph that includes all the nodes and edges.
            private FlowGraph graph;
            //level: A dictionary containing the level (distance) of each node from source. 
            //If a node does not have a level, it is not reachable by a legal path.
            private Dictionary<FlowNode, int> level;

            public DinicAlgorithm(FlowGraph graph)
            {
                this.graph = graph;
                this.level = new Dictionary<FlowNode, int>();
            }

            // Step 4.1: Building a level graph using BFS
            private bool BuildLevelGraph(FlowNode source, FlowNode sink)
            {
                //Reset the level dictionary.
                //Indicate that the source node is at level 0.
                //Put it in the queue to start BFS.
                level.Clear();
                var queue = new Queue<FlowNode>();
                level[source] = 0;
                queue.Enqueue(source);


                while (queue.Count > 0)
                {
                    // We take a node from the queue and check its neighbors.
                    var node = queue.Dequeue();

                    // נוצר עותק זמני של הקשתות כדי שלא ניגע באוסף תוך כדי מעבר
                    foreach (var edge in node.Edges.ToList())
                    {
                        var neighbor = edge.Key;
                        var capacity = edge.Value;
                        //If there is capacity (i.e., we can "flow" there),
                        //and we haven't visited this node yet (!level.ContainsKey), 
                        //then we give it one level above the level of the current node,
                        //and put it in the queue for further scanning.
                        if (capacity > 0 && !level.ContainsKey(neighbor))
                        {
                            level[neighbor] = level[node] + 1;
                            queue.Enqueue(neighbor);
                        }
                    }
                }

                //Returns whether there is at least one valid path through which a match can still be streamed.
                return level.ContainsKey(sink);
                
            }

            // Step 4.2: DFS to find a flow path-BFS
            // Step 4.2: DFS to find a flow path-BFS
            // Step 4.2: DFS to find a flow path-BFS
            private int SendFlow(FlowNode node, FlowNode sink, int flow, Dictionary<FlowNode, IEnumerator<KeyValuePair<FlowNode, int>>> iterators)
            {
                if (node == sink)
                    return flow;

                // If this is your first visit to this intersection,
                // start scanning the arches leading out of it.
                if (!iterators.ContainsKey(node))
                    // node.Edges - the edges of the current node
                    // This is an operation that returns a "transition object" – that is:
                    // Allows us to traverse all the edges of the node one by one
                    // without having to traverse the entire list again each time
                    iterators[node] = node.Edges.ToList().GetEnumerator(); // ✅ קריטי: שומר עותק כדי להימנע משגיאת שינוי בזמן מעבר

                var edges = iterators[node];

                while (edges.MoveNext())
                {
                    var next = edges.Current;
                    var neighbor = next.Key;
                    var capacity = next.Value;

                    // Conditions to ensure that this arc: still has capacity (flow can be transferred through it),
                    // the next node appears in the level graph,
                    // and is one level below (as required by Dinic)
                    if (capacity > 0 && level.ContainsKey(neighbor) && level[neighbor] == level[node] + 1)
                    {
                        int currentFlow = Math.Min(flow, capacity);
                        // Recursive read – tries to stream through the neighbor to the end (sink).
                        int tempFlow = SendFlow(neighbor, sink, currentFlow, iterators);

                        if (tempFlow > 0)
                        {
                            // מוסיפים את הזרימה לקשת ההפוכה (reverse edge) – כדי לאפשר ביטול או "החזרה" אם צריך.
                            node.Edges[neighbor] -= tempFlow;

                            if (!neighbor.Edges.ContainsKey(node))
                                neighbor.Edges[node] = 0;

                            // Reduce the capacity of the original arc on the track.
                            neighbor.Edges[node] += tempFlow;

                            // Returns how much flow we have flowed in this path.
                            return tempFlow;
                        }
                    }
                }

                return 0;
            }


            // Step 4.3: Repeat steps until exhausted -DFS

            public int MaxFlow(string sourceId, string sinkId)
            {
                var source = graph.Nodes[sourceId];
                var sink = graph.Nodes[sinkId];
                //Number of matches
                int totalFlow = 0;

                //As long as we can build a level graph, we will continue to stream.
                while (BuildLevelGraph(source, sink))
                {
                    //Create a dictionary of iterators — bookmarks to remember where each node was during the DFS.
                    //This helps avoid revisiting the same arcs.
                    var iterators = new Dictionary<FlowNode, IEnumerator<KeyValuePair<FlowNode, int>>>();
                    int flow;

                    //What happens here:
                   //We try to send a flow in a legal path(with DFS)
                  //Every time we succeed – add the amount of flow to totalFlow
                 //We go back again, and try to send more in possible paths, until there are no more(flow == 0)
                    do
                    {
                        flow = SendFlow(source, sink, int.MaxValue, iterators);
                        totalFlow += flow;
                    }
                    while (flow > 0);
                }
                //Returns the total flow – that is,
                //how many calls were actually returned to volunteers.
                return totalFlow;
            }

            // Step 4.4: Extract the substitutions (message → volunteer)
            public List<(int messageId, int volunteerId)> GetAssignments()
            {
                var result = new List<(int, int)>();

                foreach (var node in graph.Nodes.Values)
                {
                    //Check if the node is a reading node (the name starts with m_).
                    if (node.Id.StartsWith("m_"))
                    {
                        foreach (var edge in node.Edges.ToList())
                        {

                            //Let's check: Does the arch go to the volunteer (v_)
                            //Did a flow pass through it? That is, value == 0 → the capacity has been emptied = there is a match!
                           // If the capacity has dropped to zero, it means that "water has passed" - that is, the call was assigned to this volunteer.
                            if (edge.Key.Id.StartsWith("v_") && edge.Value == 0)
                            {
                                int mId = int.Parse(node.Id.Split('_')[1]);
                                int vId = int.Parse(edge.Key.Id.Split('_')[1]);
                                result.Add((mId, vId));
                            }
                        }
                    }
                }

                return result;
            }
        }
    }

}
//
// שלמסלול מה המשמעות בפועל?
//המסלול מייצג שיבוץ תקף של מתנדב לקריאה, כלומר:

//יש קריאה לעזרה (m_1)

//יש מתנדב שיכול לעזור (v_10) – כלומר עבר סינון מרחק, ידע וכו'

//לא חרגנו ממגבלת הכמות

//אז:

//כל מסלול כזה = מתנדב משובץ לקריאה מסוימת.
//