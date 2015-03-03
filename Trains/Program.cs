using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectedGraph
{
    class Program
    {
        /// <summary>
        /// Creates a controller. Reads in a file of nodes (via ReadGraph).
        /// If this is successful, passes over to the MenuHandler until
        /// the user quits the program.
        /// </summary>
        static void Main(string[] args)
        {
            FatController controller = new FatController();

            Console.WriteLine(" Welcome to the Directed Graph System! ");

            if (ReadGraph(controller))
            {
                MenuHandler(controller);
            }
            {
                Console.WriteLine("\nExiting... Goodbye!");
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Reads in a text file of connected nodes and their distances, as specified
        /// by the user. Checks for an existent, non-empty file.
        /// Files must specify one connection per line as per the format:
        /// A B 5
        /// or
        /// A,B,5
        /// where the start node, end node, and distance are separated by the delimiters
        /// ' ', ',', '.', ':', or '\t'.
        /// Checks that each connection and individual node does not yet exist in our
        /// records. If not, adds them to the controller's list of nodes and edges.
        /// Returns true when this has been managed successfully.
        /// </summary>
        public static bool ReadGraph(FatController controller)
        {
            string fileName;
            
            while (true)
            {
                Console.Write("\nEnter the name of a textfile of routes to open and press [enter]: "); // Prompt

                fileName = Console.ReadLine(); // Get string from user
                // If the file exists, break and move on.
                if (File.Exists(fileName))
                {
                    break;
                }
                Console.Write("Invalid file name, please try again.\n\n");
            }
            // Make a stream reader and open our file.
           StreamReader reader = new StreamReader(fileName);

            string line = "";

            // Define some delimiters to use to break up the lines in the file.
            char[] delimiters = { ' ', ',', '.', ':', '\t' };

            if (reader.Peek() == -1)
            {
                Console.WriteLine("\nEmpty file. No data loaded.");
                reader.Close();
                return false;
            }
            else
            {
                // While our line of input is not null
                while (line != null)
                {
                    // Get the next line in the file
                    line = reader.ReadLine();
                    

                    // As long as that line is not null.
                    if (line != null)
                    {
                        line = line.ToUpper();
                        // Split the string up into parts according to our delimiters.
                        string[] parts = line.Split(delimiters);

                        // We are not allowing node loopback, so check that the two nodes
                        // are not the same.
                        if (parts[0] == parts[1])
                        {
                            Console.WriteLine("\nInvalid file: stations may not connect back to themselves. No data loaded.");
                            reader.Close();
                            return false;
                        }

                        // Check that this start - finish edge does not exist.
                        if (!controller.DoesEdgeExist(parts[0], parts[1]))
                        {
                            // If the new nodes do not exist, add them to the node list.
                            if (controller.DoesNodeExist(parts[0]))
                            {
                                controller.nodes.Add(new Node(parts[0]));
                            }
                            if (controller.DoesNodeExist(parts[1]))
                            {
                                controller.nodes.Add(new Node(parts[1]));
                            }

                            try
                            {
                                // We already know this edge doesn't exist, so add it to our edge list.
                                controller.edges.Add(new Edge(controller.StrToNode(parts[0]),
                                    controller.StrToNode(parts[1]),
                                    Convert.ToInt32(parts[2])));
                            }
                            catch (SystemException)
                            {
                                Console.WriteLine("\nInvalid file format. No data loaded.");
                                reader.Close();
                                return false;
                            }
                        }
                    }
                }
            }
            // Close the file.
            reader.Close();
            return true;
        }

        /// <summary>
        /// Outputs the menu options.
        /// </summary>
        public static void Menu()
        {
            Console.WriteLine("\n---MENU---\n ");
            Console.WriteLine("(1) Calculate a specific route distance (e.g. From A->B->C).");
            Console.WriteLine("(2) Calculate number of stops from point A to point B with\n\ta maximum number of stops.");
            Console.WriteLine("(3) Calculate number of stops from point A to point B with\n\tan exact number of stops.");
            Console.WriteLine("(4) Calculate length of shortest route from point A to point B.");
            Console.WriteLine("(5) Calculate number of stops from point A to point B within\n\ta particular distance.");
            Console.WriteLine("(6) Print all connections.");
            Console.WriteLine("(7) Quit.");
            Console.WriteLine("\n----------");
        }

        /// <summary>
        /// Calls the Menu method to output the options. Prompts the user to choose an option
        /// Checks input, and calls the corresponding controller method. After each method
        /// has been completed, returns to the beginning of the loop. Loops until
        /// user chooses to quit the program.
        /// </summary>
        public static bool MenuHandler(FatController controller)
        {
            int selection = 0;
            bool runMenu = true;

            // While the user has not chosen to quit.
            while (runMenu)
            {
                // Print out the menu options.
                Menu();

                // Catch any non-integer exceptions - set the selection to 0
                // to force the switch statement to defer to the default.
                try
                { 
                    Console.Write("\nPlease enter your selection from 1-7 and press [enter]: "); // Prompt
                    // Get string from user, convert it to an integer.
                    selection = Convert.ToInt32(Console.ReadLine());
                }
                catch (SystemException)
                {
                    // Send switch statement to the default case.
                    selection = 0;                    
                }

                switch (selection)
                {
                    case 1:
                        // For calculating a route distance, where each node is entered
                        // by the user.
                        Console.WriteLine("\nCalculate a specific route distance (e.g. From A->B->C).");
                        RouteHandler(controller);

                        break;
                    case 2:
                        int maxStops;

                        Console.WriteLine("\nCalculate number of stops from point A to point B with\na maximum number of stops.");

                        Console.Write("\nPlease enter the name of the start station: ");
                        string nodeA = Console.ReadLine().ToUpper();
                        Console.Write("\nPlease enter the name of the end station: ");
                        string nodeB = Console.ReadLine().ToUpper();

                        // If the user just hit [enter], break and loop back.
                        if (nodeA == "" || nodeB == "")
                        {
                            Console.WriteLine("\nInvalid station name.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Exception check for non-int input.
                        try
                        {
                            Console.Write("\nPlease enter the maximum number of stops allowed: ");
                            maxStops = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("\nInvalid input for maximum number of stops.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Call MaxStops.
                        controller.MaxStops(controller.StrToNode(nodeA),
                            controller.StrToNode(nodeB),
                            maxStops);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();

                        break;
                    case 3:
                        int exactStops;

                        Console.WriteLine("\nCalculate number of stops from point A to point B with\nan exact number of stops.");

                        Console.Write("\nPlease enter the name of the start station: ");
                        string exactNodeA = Console.ReadLine().ToUpper();
                        Console.Write("\nPlease enter the name of the end station: ");
                        string exactNodeB = Console.ReadLine().ToUpper();

                        // If the user just hit [enter], break and loop back.
                        if (exactNodeA == "" || exactNodeB == "")
                        {
                            Console.WriteLine("\nInvalid station name.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Exception check for non-int input.
                        try
                        {
                            Console.Write("\nPlease enter the exact number of stops allowed: ");
                            exactStops = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("\nInvalid input for number of stops.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Call ExactStops.
                        controller.ExactStops(controller.StrToNode(exactNodeA),
                            controller.StrToNode(exactNodeB),
                            exactStops);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();

                        break;
                    case 4:
                        Console.WriteLine("\nCalculate length of shortest route from point A to point B.");

                        Console.Write("\nPlease enter the name of the start station: ");
                        string shortNodeA = Console.ReadLine().ToUpper();
                        Console.Write("\nPlease enter the name of the end station: ");
                        string shortNodeB = Console.ReadLine().ToUpper();

                        // If the user just hit [enter], break and loop back.
                        if (shortNodeA == "" || shortNodeB == "")
                        {
                            Console.WriteLine("\nInvalid station name.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Call PrintShortestPath
                        controller.PrintShortestPath(controller.StrToNode(shortNodeA),
                            controller.StrToNode(shortNodeB));

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        
                        break;
                    case 5:
                        int dist;

                        Console.WriteLine("\nCalculate number of stops from point A to point B within\na particular distance.");

                        Console.Write("\nPlease enter the name of the start station: ");
                        string distNodeA = Console.ReadLine().ToUpper();
                        Console.Write("\nPlease enter the name of the end station: ");
                        string distNodeB = Console.ReadLine().ToUpper();

                        // If the user just hit [enter], break and loop back.
                        if (distNodeA == "" || distNodeB == "")
                        {
                            Console.WriteLine("\nInvalid station name.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Exception check for non-int input.
                        try
                        {
                            Console.Write("\nPlease enter the maximum distance allowed: ");
                            dist = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (SystemException)
                        {
                            Console.WriteLine("\nInvalid input for maximum distance.\nReturning to menu.");
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        }

                        // Call DifferentStopsUnder.
                        controller.DifferentStopsUnder(controller.StrToNode(distNodeA),
                            controller.StrToNode(distNodeB),
                            dist);

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case 6:
                        // Call PrintAllEdges to print all the connections in the map.
                        controller.PrintAllEdges();

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();

                        break;
                    case 7:
                        // Quit scenario. Returns to main function and ends program.
                        runMenu = false;
                        return false;

                    default:
                        // Outputs invalid input message and loops back.
                        Console.Write("\nInvalid selection, please try again.\n\n");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                }
            }
            return true;
        }

        /// <summary>
        /// Allows the user to input nodes to create a route, until the user hits [enter].
        /// (If the user hits [enter] before ANY nodes have been entered, the method
        /// will flag that as an invalid node and return to the main menu.) The nodes are added
        /// to a list of nodes, and that list is sent to the controller's method PrintRouteDist
        /// to calculate and print the distance of nodes in that list. Finally, returns to
        /// main menu (MenuHandler).
        /// </summary>
        public static void RouteHandler(FatController controller)
        {
            bool enterMore = true;
            string node;
            
            List<string> nodeList = new List<string>();

            // While we're still taking in nodes to add to the route.
            while (enterMore)
            {
                Console.Write("\nPlease enter the name of a station and press [enter].\nEnter nothing and press [enter] to end a route.: "); // Prompt
                node = Console.ReadLine();
                
                // If nothing has been entered, we've finished reading in nodes.
                // Otherwise, add the inputted node to the route list.
                if (node == "")
                {
                    enterMore = false;
                }
                else
                {
                    node = node.ToUpper();
                    nodeList.Add(node);
                }
            }

            List<Node> fnlNodeList = new List<Node>();

            foreach(string n in nodeList)
            {
                if (n != "")
                {
                    // Add each node to our node list
                    fnlNodeList.Add(controller.StrToNode(n));
                }
            }

            // Send the node list off to RouteDist to calculate the distance.
            if (fnlNodeList.Count > 0)
            {
                controller.PrintRouteDist(fnlNodeList);
            }
            else
            {
                Console.WriteLine("\nNo stations entered. Returning to menu.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }
    }
}

    
