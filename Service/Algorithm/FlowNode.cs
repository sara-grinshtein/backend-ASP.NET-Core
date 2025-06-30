using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Algorithm
{
    public class FlowNode
    {
        public string Id { get; set; }
        public Dictionary<FlowNode, int> Edges { get; set; } = new(); // יעד → קיבולת
    }
}
