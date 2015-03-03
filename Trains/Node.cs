using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraph
{
    /// <summary>
    /// Very basic node class, will hold only the name of a node.
    /// </summary>
    public class Node
    {
        string name { get; set; }

        /// <summary>
        /// Creates a blank node.
        /// </summary>
        public Node() { }

        /// <summary>
        /// Creates a node and sets it's name.
        /// </summary>
        public Node(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Returns the name of the node.
        /// </summary>
        public string Name()
        {
            return name;
        }
    }
}
