using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Algorithm
{
    public class FlowGraph
    {
        //Nodes: A dictionary of all nodes by ID name (Id).
        public Dictionary<string, FlowNode> Nodes { get; set; } = new();

        //AddNode: Adds a node if it does not already exist.
        public void AddNode(string id)
        {
            if (!Nodes.ContainsKey(id))
            {
                Nodes[id] = new FlowNode { Id = id };
            }
        }
        //AddEdge: Adds an edge from fromId to toId with a certain capacity.
        public void AddEdge(string fromId, string toId, int capacity)
        {
            AddNode(fromId);
            AddNode(toId);

            var fromNode = Nodes[fromId];
            var toNode = Nodes[toId];

            if (!fromNode.Edges.ContainsKey(toNode))
            {
                fromNode.Edges[toNode] = capacity;
            }
        }
    }

}
