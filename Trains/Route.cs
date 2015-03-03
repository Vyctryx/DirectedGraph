using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraph
{
    public class Route
    {
        public List<Node> routeNodes;
        public int routeDistance;

        /// <summary>
        /// Creates a "blank" route class - gives it an empty list of
        /// nodes and a distance of zero.
        /// </summary>
        public Route()
        {
            this.routeNodes = new List<Node>();
            routeDistance = 0;
        }

        /// <summary>
        /// Makes a deep copy of an old route to save memory issues occuring.
        /// </summary>
        public Route(Route oldRoute)
        {
            this.routeNodes = new List<Node>(oldRoute.routeNodes);
            this.routeDistance = oldRoute.routeDistance;
        }

        /// <summary>
        /// Check whether a node exists in our route's list of nodes.
        /// Returns true or false.
        /// </summary>
        public bool Contains(Node node)
        {
            foreach (Node thisNode in routeNodes)
            {
                if (thisNode.Name() == node.Name())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
