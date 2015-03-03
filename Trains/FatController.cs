using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectedGraph
{
    /// <summary>
    /// A controller class that handles all of our node and edge calculations
    /// and operations.
    /// </summary>
    public class FatController
    {
        // list of Nodes
        public List<Node> nodes { get; set; }
        // list of Edges
        public List<Edge> edges { get; set; }

        /// <summary>
        /// Creates a new controller, with a new list of nodes and a
        /// new list of edges.
        /// </summary>
        public FatController()
        {
            nodes = new List<Node>();
            edges = new List<Edge>();
        }

        /// <summary>
        /// Iterates through our list of nodes to check if a node exists 
        /// in our controller's list of nodes. Returns true or false.
        /// </summary>
        public bool DoesNodeExist(string name)
        {
            // possibly use a dictionary <string, Node>
            // as this spends a lot of time here.
            foreach (Node thisNode in nodes)
            {
                if (thisNode.Name() == name)
                { 
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Takes in a string node name, and returns the corresponding node.
        /// Also creates a new node if the specified node does not already exist.
        /// (This is not great and I want to eliminate the need for it.
        /// Such iteration. Many process. Wow.)
        /// </summary>
        public Node StrToNode (string nodeName)
        {
            foreach (Node thisNode in nodes)
            {
                if (thisNode.Name() == nodeName)
                {
                    return thisNode;
                }
            }

            Node newNode = new Node(nodeName);
            nodes.Add(newNode);
            return newNode;
        }

        /// <summary>
        /// Iterate through our controller's list of edges to check 
        /// if an edge already exists. Returns true or false.
        /// </summary>
        public bool DoesEdgeExist(string nameA, string nameB)
        {
            foreach (Edge thisEdge in edges)
            {
                if (thisEdge.startNode.Name()== nameA &&
                    thisEdge.endNode.Name() == nameB)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Iterates through the controller's list of edges and prints out
        /// the information for each, via a call to our Edge class' PrintEdge
        /// method.
        /// </summary>        
        public void PrintAllEdges()
        {
            Console.WriteLine("\nConnections in map:\n");
            
            // Iterate through our list of edges.
            foreach (Edge element in edges)
            {
                Console.WriteLine(element.PrintEdge());
            }
        }

        /// <summary>
        /// Creates a list of edges to store all the "neighbours" of a 
        /// given node. Goes through our list of edges and checks the name
        /// of each start node. If it matches the name of the node we've passed
        /// in, we add that edge to list of neighbours. Returns the list of edges
        /// once finished. (Empty lists are allowed to be returned.)
        /// </summary>
        public List<Edge> GetNeighbourEdges(Node node)
        {
            List<Edge> neighbours = new List<Edge>();

            foreach (Edge nextEdge in edges)
            {
                if (node.Name() == nextEdge.startNode.Name())
                {
                    neighbours.Add(nextEdge);
                }
            }
            return neighbours;
        }

        /// <summary>
        /// FindRouteDist method for if a list of strings has been passed in, rather than
        /// a list of nodes. Converts the strings to their node equivalents, stores them 
        /// in a list and sends it to the node version of the method.
        /// Returns 0 if no route is found.
        /// </summary> 
        public int FindRouteDist(List<string> nodeList)
        {
            // Create a new list of nodes.
            List<Node> newNodeList = new List<Node>();

            // For each string in the list, find that node and add it to our list of nodes.
            foreach (string name in nodeList)
            {
                newNodeList.Add(new Node(name));
            }

            // Call the List<Node> version of this method and return the result from that.
            return FindRouteDist(newNodeList);
        }

        /// <summary>
        /// Goes through our list of nodes. Checks the proceeding node are neighbours
        /// via a call to GetNeighbourEdges. If they are not neighbours, returns 0 to
        /// signify that the route does not exist. If they are neighbours, adds the distance
        /// of the edge to the overall distance. Returns the final distance.
        /// </summary>
        public int FindRouteDist(List<Node> nodeList)
        {
            // Create a blank list to hold a list of neighbours to the "current" node.
            List<Edge> currentNeighbours = new List<Edge>();
            // Initialise our total distance to 0. 
            int total = 0;

            // Iterate through the list of nodes passed in; stop 1 before the end of the list
            // to prevent overrun.
            for (int i = 0; i < nodeList.Count() - 1; i++)
            {
                // Current node in the list
                Node currentNode = nodeList[i];
                // The next node in the list
                Node nextNode = nodeList[i + 1];

                // Populate our neighbour list with all the nodes connected to the current node.
                currentNeighbours = GetNeighbourEdges(currentNode);
                
                // To make sure our route is valid! 
                bool found = false;
                
                // For each of the connections in the neighbours list
                foreach (Edge nextEdge in currentNeighbours)
                {
                    // If any of the neighbours match the next node in our node list
                    if (nextEdge.endNode.Name() == nextNode.Name())
                    {
                        // Add the distance to our accumulated distance
                        // Flag that our route is valid.
                        total += nextEdge.distance;
                        found = true;
                    }
                }
                if (!found) return 0;
            }         
            // If we reach this point, the route exists and we can return the route distance.
            return total;
        }

        /// <summary>
        /// Sends a list of nodes to FindRouteDist. If the returned result is 0,
        /// the route does not exist. Output a NO ROUTE message. Otherwise, output
        /// the distance of the route as specified in the list of nodes.
        /// </summary>
        public void PrintRouteDist (List<Node> nodeList)
        {
            // Call our distance calculating method. It will return 
            // 0 if the route doesn't exist.
            int distance = FindRouteDist(nodeList);
            int ctr = 1;

            Console.Write("\nThe distance of the route \"");

            // First we'll output the nodes in the route.
            foreach (Node node in nodeList)
             {   
                Console.Write("{0}", node.Name());
                if(ctr != nodeList.Count)
                {
                    Console.Write(" -> ");
                }
                ctr++;
            }
            // Then we'll either output the distance or flag that the route doesn't exist.
            if (distance != 0)
            {
                Console.Write("\" is {0}.\n", distance);
            }
            else
            {
                Console.Write("\" is null. NO SUCH ROUTE.\n\n");
            }
        }

        /// <summary>
        /// Takes in a start node and an end node, and returns a list of Routes containing
        /// all possible routes between the two and their corresponding distances/maximum stops.
        /// 
        /// An example of the algorithm in action is provided in the README file.
        /// </summary>
        public List<Route> GetAllPaths(Node startNode, Node endNode, bool allowsRevisiting, 
            bool allowsLoopback, bool goalState, int maxStops, int maxDist)
        {
            // Create a blank list of visited nodes, and a blank list
            // of possible routes.
            List<Route> possibleRoutes = new List<Route>();
            List<Route> finalRoutes = new List<Route>();

            // Create a new route and add the starting node to it
            Route route = new Route();
            route.routeNodes.Add(startNode);

            // Add this route to our list of possible routes
            possibleRoutes.Add(route);

            // While we still have routes to process
            while (possibleRoutes.Count != 0)
            {
                List<Route> newPossibleRoutes = new List<Route>();

                // While we have possible routes to continue adding to.
                foreach (Route thisRoute in possibleRoutes)
                {
                    // Get the last item in the possible route list
                    Node thisNode = thisRoute.routeNodes[thisRoute.routeNodes.Count - 1];

                    List<Edge> edgeList = new List<Edge>();
                    // Get the current node's neighbours
                    edgeList = GetNeighbourEdges(thisNode);

                    foreach (Edge thisEdge in edgeList)
                    {
                        // Check to see if we've allowed start nodes and end nodes to be the same node.
                        bool isPossibleRoute = !thisRoute.Contains(thisEdge.endNode);
                        if (allowsLoopback)
                        {
                            isPossibleRoute |= startNode.Name() == thisEdge.endNode.Name();
                        }
                        isPossibleRoute |= allowsRevisiting;

                        // If we're checking a max distance...
                        if (maxDist > 0)
                        {
                            // If the distance of the current route we're examining
                            // has exceeded the maximum distance, this is not a possible
                            // route.
                            if(thisRoute.routeDistance > maxDist)
                            { 
                                isPossibleRoute = false;
                            }
                         }
                        // If we're checking for a maximum number of stops...
                        if (maxStops > 0)
                        {
                            // If the number of stops of the current route we're examining
                            // has exceeded the maximum number of stops, this is not a 
                            // possible route.
                            if (thisRoute.routeNodes.Count > maxStops)
                            {
                                isPossibleRoute = false;
                            }
                        }
                        // If the end node of this edge is not in this thisRoute,
                        // add this end node to possibleRoutes.
                        if (isPossibleRoute)
                        {
                            // Make a deep copy to avoid memory issues.
                            Route updatedRoute = new Route(thisRoute);
                            updatedRoute.routeNodes.Add(thisEdge.endNode);
                            updatedRoute.routeDistance += thisEdge.distance;

                            // Turnary; says that if our goal state is set to true,
                            // then if our current neighbour being examined is the same
                            // as our end node, we can set isComplete to true. If our 
                            // goal state is set to false, isComplete is set to false.
                            bool isComplete = goalState ? thisEdge.endNode.Name() == endNode.Name() : false;
                            
                            // Check our maximum stops again.
                            // Can I move this to the previous check as it's a repeat check?
                            // Or will that somehow break everything because *dark magic*?
                            // (Or because we're within an entirely different if-statement?)
                            if (maxStops > 0)
                            {
                                if (updatedRoute.routeNodes.Count >= maxStops)
                                {
                                    // We've reach one of our terminating conditions.
                                    isComplete = true;
                                }
                            }
                            
                            // If we've reached our terminating condition and the current
                            // neighbour node being examined is the end node we're after...
                            if (isComplete && thisEdge.endNode.Name() == endNode.Name())
                            {
                                // Do this if we're looking for a maximum distance.
                                if (maxDist > 0)
                                {
                                    // If our current route's distance is under the max 
                                    // distance allowed...
                                    if (updatedRoute.routeDistance <= maxDist)
                                    {
                                        // Add to our possible routes.
                                        newPossibleRoutes.Add(new Route(updatedRoute));
                                    }
                                }
                                // Add to our FINAL possible routes as this is a complete
                                // and valid route.
                                finalRoutes.Add(updatedRoute);
                            }
                            else
                            {
                                // Add it to our ongoing list of possible routes to be
                                // added to (or discarded) in future iterations.
                                newPossibleRoutes.Add(updatedRoute);
                            }
                        }
                    }
                }
                // Update the old list of possible routes with our new lists
                possibleRoutes = newPossibleRoutes;
            }
            // Return the final list of possible routes! :D
            return finalRoutes;
        }

        /// <summary>
        /// Iterates through the list of routes that has been passed
        /// in. If the distance of the current route is smaller than our
        /// smallest recorded distance, make this our new smallest distance.
        /// Otherwise, move on. (Smallest recorded distance is set to MaxValue
        /// for the first comparison.) Return the smallest distance found.
        /// </summary>
        public int GetShortestPath(List<Route> routeList)
        {
            // Set our initial shortest path distance to as close to infinity as we can
            // manage, so that any real path values we find in our possible route list will
            // have to be smaller. :)
            int shortestPath = int.MaxValue;

            // Go through each of our final routes.
            foreach (Route route in routeList)
            {
                // If the distance of that route is smaller than the smallest
                // distance we have recorded, discard the old value and save
                // this value.
                if(route.routeDistance < shortestPath)
                {
                    shortestPath = route.routeDistance;
                }
            }
            return shortestPath;
        }

        /// <summary>
        /// Sends the nodes off to getAllPaths. If there are no possible
        /// routes between the start and end node, displays error message. Otherwise
        /// sends this list off to find the distance of the shortest route.
        /// </summary>
        public void PrintShortestPath (Node startNode, Node endNode)
        {
            List<Route> routeList = new List<Route>();

            // Get all the possible routes between a start and end node.
            routeList = GetAllPaths(startNode, endNode, false, true, true, 0, 0);

            // If we have possible routes from start to end nodes
            if(routeList.Count != 0)
            {
                // Get the distance of the shortest route
                int shortestPath = GetShortestPath(routeList);
                // Output that information
                Console.WriteLine("\nThe distance of the shortest route for route {0} -> {1}: {2}.",
                    startNode.Name(),
                    endNode.Name(),
                    shortestPath);
                return;
            }

            // Otherwise this route does not exist. Output that.
            NonExistantRoute(startNode, endNode);
        }

        /// <summary>
        /// Finds the number of all possible paths between a start and an end node for a
        /// given maximum number of stops and outputs the result.
        /// </summary>
        public void MaxStops(Node startNode, Node endNode, int maxStops)
        {
            List<Route> routeList = new List<Route>();
            // Get a list of all possible routes from the start to end nodes.
            routeList = GetAllPaths(startNode, endNode, true, true, true, maxStops, 0);
            int totalNoOfStops = 0;

            // If we have possible routes from start to end nodes
            if (routeList.Count != 0)
            {
                // For each of our routes
                foreach (Route route in routeList)
                {
                    // If the number of nodes in this route is less than the maximum
                    // stops allowed, increment our number of stops counter.
                    if((route.routeNodes.Count - 1) <= maxStops)
                    {
                        totalNoOfStops++;
                    }
                }
                // Output our results.
                Console.WriteLine("\nTrips beginning at {0} and ending at {1} with a maximum of {2} stops: {3}.",
                    startNode.Name(),
                    endNode.Name(),
                    maxStops,
                    totalNoOfStops);

                return;
            }
            NonExistantRoute(startNode, endNode);
        }

        /// <summary>
        /// Finds the number of all possible paths between a start and an end node for a
        /// given minimum number of stops and outputs the result. INCOMPLETE.
        /// </summary>       
        public void MinStops(Node startNode, Node endNode, int minStops)
        {
            List<Route> routeList = new List<Route>();
            // Get a list of all possible routes from the start to end nodes.
            routeList = GetAllPaths(startNode, endNode, false, true, true, 0, 0);
            int totalNoOfStops = 0;

            // If we have possible routes from start to end nodes
            if (routeList.Count != 0)
            {
                // For each of our routes
                foreach (Route route in routeList)
                {
                    // If the number of nodes in this route is greater than the minimum
                    // stops allowed, increment our number of stops counter.
                    if ((route.routeNodes.Count - 1) >= minStops)
                    {
                        totalNoOfStops++;
                    }
                }
                // Output our results.
                Console.WriteLine("\nTrips beginning at {0} and ending at {1} with a maximum of {2} stops: {3}.",
                    startNode.Name(),
                    endNode.Name(),
                    minStops,
                    totalNoOfStops);

                return;
            }
            NonExistantRoute(startNode, endNode);
        }

        /// <summary>
        /// Finds the number of all possible paths between a start and an end node for an
        /// exact given number of stops and outputs the results.
        /// </summary> 
        public void ExactStops(Node startNode, Node endNode, int exactStops)
        {
            List<Route> routeList = new List<Route>();
            // Get a list of all possible routes from the start to end nodes.
            routeList = GetAllPaths(startNode, endNode, true, true, false, exactStops, 0);
            int totalNoOfStops = 0;

            // If we have possible routes from start to end nodes
            if (routeList.Count != 0)
            {
                // For each of our routes
                foreach (Route route in routeList)
                {
                    // If the number of nodes in this route is greater than the minimum
                    // stops allowed, increment our number of stops counter.
                    if ((route.routeNodes.Count - 1) == exactStops)
                    {
                        totalNoOfStops++;
                    }
                }
                // Output our results.
                Console.WriteLine("\nTrips beginning at {0} and ending at {1} with exactly {2} stops: {3}.",
                    startNode.Name(),
                    endNode.Name(),
                    exactStops,
                    totalNoOfStops);

                return;
            }
            NonExistantRoute(startNode, endNode);
        }

        /// <summary>
        /// Finds the number of all possible paths between a start and an end node for a
        /// given maximum distance, and outputs the result.
        /// </summary> 
        public void DifferentStopsUnder(Node startNode, Node endNode, int maxDist)
        {
            List<Route> routeList = new List<Route>();
            // Get a list of all possible routes from the start to end nodes.
            routeList = GetAllPaths(startNode, endNode, true, true, true, 0, maxDist);
            int totalNoOfRoutes = 0;

            // If we have possible routes from start to end nodes
            if (routeList.Count != 0)
            {
                // For each of our routes
                foreach (Route route in routeList)
                {
                    // If the number of nodes in this route is less than the maximum
                    // stops allowed, increment our number of stops counter.
                    if (route.routeDistance < maxDist)
                    {
                        totalNoOfRoutes++;
                    }
                }
                // Output our results.
                Console.WriteLine("\nTrips beginning at {0} and ending at {1} with a distance of less than {2}: {3}.",
                    startNode.Name(),
                    endNode.Name(),
                    maxDist,
                    totalNoOfRoutes);

                return;
            }
            NonExistantRoute(startNode, endNode);
        }

        /// <summary>
        /// Outputs an error message for non-existent routes.
        /// </summary> 
        public void NonExistantRoute(Node startPoint, Node endPoint)
        {
            Console.WriteLine("NO SUCH ROUTE: {0} -> {1}",
               startPoint.Name(),
               endPoint.Name());
        }      
    }
}