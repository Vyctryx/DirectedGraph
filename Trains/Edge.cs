using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraph
{
    /// <summary>
    /// Contains information about the edges: a start node, an end node, and the distance of that node.
    /// </summary>
    public class Edge
    {
        public Node startNode { get; set; }
        public Node endNode { get; set; }
        public int distance { get; set; }

        /// <summary>
        /// Creates a new "edge", or connection. Sets it's start and end nodes,
        /// and the distance between them.
        /// </summary>
        public Edge(Node start, Node end, int dist)
        {
            this.startNode = start;
            this.endNode = end;
            this.distance = dist;
        }

        /// <summary>
        /// Formats our stored information about our edges and returns it
        /// as a string.
        /// </summary>
        public string PrintEdge()
        {
            return String.Format("{0} -> {1}, Distance: {2}",
                            this.startNode.Name(),
                            this.endNode.Name(),
                            this.distance);
        }
    }
}
