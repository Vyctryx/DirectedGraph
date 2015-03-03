using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectedGraph;

namespace DirectedGraphTest
{
    [TestClass]
    public class FatControllerTests
    {
        [TestMethod]
        // Test that our FatController constructor works.
        public void TestFCConstructor()
        {
            FatController controller = new FatController();

            // Check blank controller
            Assert.IsNotNull(controller);
            Assert.AreEqual(controller.nodes.Count, 0);
            Assert.AreEqual(controller.edges.Count, 0);

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            int dist = 4;

            Edge edge = new Edge(nodeA, nodeB, dist);

            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.edges.Add(edge);

            // Check non-blank controller
            Assert.AreEqual(controller.nodes.Count, 2);
            Assert.AreEqual(controller.edges.Count, 1);
        }

        [TestMethod]
        // Test that DoesNodeExists is returning the correct result when
        // checking whether a node exists in the controllers Node list.
        public void TestFCDoesNodeExist()
        {
            FatController controller = new FatController();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");

            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);

            bool testNodeA = controller.DoesNodeExist("A");
            bool testNodeB = controller.DoesNodeExist("B");
            bool testNodeC = controller.DoesNodeExist("C");

            Assert.IsTrue(testNodeA);
            Assert.IsTrue(testNodeB);
            Assert.IsFalse(testNodeC);
        }

        [TestMethod]
        // Test that our FatController constructor works.
        public void TestFCDoesEdgeExist()
        {
            FatController controller = new FatController();

            // Check blank controller
            Assert.IsNotNull(controller);
            Assert.AreEqual(controller.nodes.Count, 0);
            Assert.AreEqual(controller.edges.Count, 0);

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            int dist = 4;

            Edge edge = new Edge(nodeA, nodeB, dist);

            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.edges.Add(edge);

            bool testEdgeA = controller.DoesEdgeExist("A", "B");
            bool testEdgeB = controller.DoesEdgeExist("B", "A");
            bool testEdgeC = controller.DoesEdgeExist("B", "C");

            Assert.IsTrue(testEdgeA);
            Assert.IsFalse(testEdgeB);
            Assert.IsFalse(testEdgeC);
        }

        [TestMethod]
        // To test that our StrToNode method works as expected.
        public void TestFCStrToNode()
        {
            FatController controller = new FatController();

            Node node = new Node("A");
            Node blankNode = new Node();
            Node otherBlankNode = new Node();

            controller.nodes.Add(node);

            blankNode = controller.StrToNode("A");
            otherBlankNode = controller.StrToNode("B");

            Assert.IsNotNull(blankNode);
            Assert.AreEqual(blankNode.Name(), "A");
            // This should be true because it will create the node and
            // return it if it did not already exist.
            Assert.IsNotNull(otherBlankNode);
            Assert.AreEqual(otherBlankNode.Name(), "B");        
        }

        [TestMethod]
        // To test that our GetNeighbourEdges method returns
        // the correct neighbouring edges for a given node.
        public void TestFCGetNeighbourEdges()
        {
            // Set up some nodes and a controller
            
            FatController controller = new FatController();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");
            int distAB = 4;
            int distAC = 3;
            int distCA = 2;

            Edge edgeAB = new Edge(nodeA, nodeB, distAB);
            Edge edgeAC = new Edge(nodeA, nodeC, distAC);
            Edge edgeCA = new Edge(nodeC, nodeA, distCA);

            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.nodes.Add(nodeC);
            controller.edges.Add(edgeAB);
            controller.edges.Add(edgeAC);
            controller.edges.Add(edgeCA);

            // Lists to hold the neighbours for each node
            List<Edge> aNeighbours = new List<Edge>();
            List<Edge> bNeighbours = new List<Edge>();
            List<Edge> cNeighbours = new List<Edge>();

            // Get the neighbours
            aNeighbours = controller.GetNeighbourEdges(nodeA);
            bNeighbours = controller.GetNeighbourEdges(nodeB);
            cNeighbours = controller.GetNeighbourEdges(nodeC);
            
            // Make sure the lists contain the right # of neighbours and the correct neighbours!
            Assert.AreEqual(aNeighbours.Count, 2);
            Assert.AreEqual(bNeighbours.Count, 0);
            Assert.AreEqual(cNeighbours.Count, 1);
            Assert.AreEqual(aNeighbours[0].endNode.Name(), "B");
            Assert.AreEqual(aNeighbours[1].endNode.Name(), "C");
            Assert.AreEqual(cNeighbours[0].endNode.Name(), "A");
        }

        [TestMethod]
        // To test that the string list version of our FindRouteDist method
        // correctly sends the corresponding node list of to the node list
        // version of the method.
        public void TestFCStrFindRouteDist()
        {
            // Set up some nodes and a controller

            FatController controller = new FatController();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");
            int distAB = 4;
            int distAC = 3;
            int distCA = 2;

            Edge edgeAB = new Edge(nodeA, nodeB, distAB);
            Edge edgeAC = new Edge(nodeA, nodeC, distAC);
            Edge edgeCA = new Edge(nodeC, nodeA, distCA);

            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.nodes.Add(nodeC);
            controller.edges.Add(edgeAB);
            controller.edges.Add(edgeAC);
            controller.edges.Add(edgeCA);

            // Create lists of (string) nodes in a route.
            List<string> strListA = new List<string>() { "A", "C", "A" }; // valid route
            List<string> strListB = new List<string>() { "B", "C"}; // invalid route
            List<string> strListC = new List<string>() { "C"}; // invalid route
            List<string> strListD = new List<string>(); // invalid route

            int totDistA = controller.FindRouteDist(strListA);
            int totDistB = controller.FindRouteDist(strListB);
            int totDistC = controller.FindRouteDist(strListC);
            int totDistD = controller.FindRouteDist(strListD);

            Assert.AreEqual(totDistA, 5);
            Assert.AreEqual(totDistB, 0);
            Assert.AreEqual(totDistC, 0);
            Assert.AreEqual(totDistD, 0);
        }

        [TestMethod]
        // To test that the node list version of our FindRouteDist method
        // correctly calculates the distance of any given node list.
        public void TestFCFindRouteDist()
        {
            // Set up some nodes and a controller

            FatController controller = new FatController();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");
            int distAB = 4;
            int distAC = 3;
            int distCA = 2;

            Edge edgeAB = new Edge(nodeA, nodeB, distAB);
            Edge edgeAC = new Edge(nodeA, nodeC, distAC);
            Edge edgeCA = new Edge(nodeC, nodeA, distCA);

            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.nodes.Add(nodeC);
            controller.edges.Add(edgeAB);
            controller.edges.Add(edgeAC);
            controller.edges.Add(edgeCA);

            // Create lists of nodes in a route.
            List<Node> listA = new List<Node>() { nodeA, nodeC, nodeA }; // valid route
            List<Node> listB = new List<Node>() { nodeB, nodeC }; // invalid route
            List<Node> listC = new List<Node>() { nodeC }; // invalid route
            List<Node> listD = new List<Node>(); // invalid route

            int totDistA = controller.FindRouteDist(listA);
            int totDistB = controller.FindRouteDist(listB);
            int totDistC = controller.FindRouteDist(listC);
            int totDistD = controller.FindRouteDist(listD);

            Assert.AreEqual(totDistA, 5);
            Assert.AreEqual(totDistB, 0);
            Assert.AreEqual(totDistC, 0);
            Assert.AreEqual(totDistD, 0);
        }

        [TestMethod]
        // To test that GetAllPaths works correctly in all scenarios.
        public void TestFCGetAllPaths()
        {
            // Set up some nodes (duplicating the assignment test data)
            // and a controller.
            FatController controller = new FatController();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");
            Node nodeD = new Node("D");
            Node nodeE = new Node("E");
            Node nodeF = new Node("F");
            Node nodeG = new Node("G");
            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.nodes.Add(nodeC);
            controller.nodes.Add(nodeD);
            controller.nodes.Add(nodeE);
            controller.nodes.Add(nodeF);
            controller.nodes.Add(nodeG);
            controller.edges.Add(new Edge(nodeA, nodeB, 5));
            controller.edges.Add(new Edge(nodeB, nodeC, 4));
            controller.edges.Add(new Edge(nodeC, nodeD, 8));
            controller.edges.Add(new Edge(nodeD, nodeC, 8));
            controller.edges.Add(new Edge(nodeD, nodeE, 6));
            controller.edges.Add(new Edge(nodeA, nodeD, 5));
            controller.edges.Add(new Edge(nodeC, nodeE, 2));
            controller.edges.Add(new Edge(nodeE, nodeB, 3));
            controller.edges.Add(new Edge(nodeA, nodeE, 7));
            controller.edges.Add(new Edge(nodeF, nodeG, 7));
            controller.edges.Add(new Edge(nodeG, nodeF, 6));

            // Create lists of nodes in possible routes from a start to end node.
            // First we'll test this for shortest paths.
            List<Route> listACshort = new List<Route>();
            List<Route> listCCshort = new List<Route>();
            List<Route> listDGshort = new List<Route>();

            // False because for shortest paths we don't allow node revisiting.
            // 0 and 0 because we're not considering a maximum number of stops,
            // nor a maximum distance.
            listACshort = controller.GetAllPaths(nodeA, nodeC, false, true, true, 0, 0); // valid
            listCCshort = controller.GetAllPaths(nodeC, nodeC, false, true, true, 0, 0); // valid
            listDGshort = controller.GetAllPaths(nodeD, nodeG, false, true, true, 0, 0); //invalid

            // Check the amount of possible routes returned is correct.
            Assert.AreEqual(listACshort.Count, 4);
            Assert.AreEqual(listCCshort.Count, 3);
            Assert.AreEqual(listDGshort.Count, 0);

            // Now check possible routes for a maximum number of stops.
            List<Route> listCCMaxStops = new List<Route>();
            List<Route> listDGMaxStops = new List<Route>();

            // True because for maximum stop paths we allow node revisiting.
            listCCMaxStops = controller.GetAllPaths(nodeC, nodeC, true, true, true, 3, 0); // valid
            listDGMaxStops = controller.GetAllPaths(nodeD, nodeG, true, true, true, 4, 0); //invalid

            // Check the amount of possible routes returned is correct.
            Assert.AreEqual(listCCMaxStops.Count, 2);
            Assert.AreEqual(listDGMaxStops.Count, 0);

            // Now check possible routes for an exact number of stops.
            List<Route> listACExactStops = new List<Route>();
            List<Route> listDGExactStops = new List<Route>();

            listACExactStops = controller.GetAllPaths(nodeA, nodeC, true, true, false, 4, 0); // valid
            listDGExactStops = controller.GetAllPaths(nodeD, nodeG, true, true, false, 4, 0); //invalid

            // Check the amount of possible routes returned is correct.
            // Even this returns 4, it is still the correct number of routes it should
            // be returning. ExactStops will exctract the correct number of routes
            // with the exact number of required stops.
            Assert.AreEqual(listACExactStops.Count, 4);
            Assert.AreEqual(listDGExactStops.Count, 0);

            // Now check possible routes for a maximum distance.
            List<Route> listCCDist = new List<Route>();
            List<Route> listDGDist = new List<Route>();

            // True because for maximum stop paths we allow node revisiting.
            listCCDist = controller.GetAllPaths(nodeC, nodeC, true, true, true, 0, 30); // valid
            listDGDist = controller.GetAllPaths(nodeD, nodeG, true, true, true, 0, 30); //invalid

            // Check the amount of possible routes returned is correct.
            // Even though this returns 14, it is still the correct number of routes 
            // it should be returning. DifferentStopsUnder will exctract the correct 
            // number of routes below the given distance.
            Assert.AreEqual(listCCDist.Count, 14);
            Assert.AreEqual(listDGDist.Count, 0);
        }

        [TestMethod]
        // To test that GetAllPaths works correctly in all scenarios.
        public void TestFCGetShortestPath()
        {
            // Set up some nodes (duplicating the assignment test data)
            // and a controller.
            FatController controller = new FatController();

            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");
            Node nodeD = new Node("D");
            Node nodeE = new Node("E");
            Node nodeF = new Node("F");
            Node nodeG = new Node("G");
            controller.nodes.Add(nodeA);
            controller.nodes.Add(nodeB);
            controller.nodes.Add(nodeC);
            controller.nodes.Add(nodeD);
            controller.nodes.Add(nodeE);
            controller.nodes.Add(nodeF);
            controller.nodes.Add(nodeG);
            controller.edges.Add(new Edge(nodeA, nodeB, 5));
            controller.edges.Add(new Edge(nodeB, nodeC, 4));
            controller.edges.Add(new Edge(nodeC, nodeD, 8));
            controller.edges.Add(new Edge(nodeD, nodeC, 8));
            controller.edges.Add(new Edge(nodeD, nodeE, 6));
            controller.edges.Add(new Edge(nodeA, nodeD, 5));
            controller.edges.Add(new Edge(nodeC, nodeE, 2));
            controller.edges.Add(new Edge(nodeE, nodeB, 3));
            controller.edges.Add(new Edge(nodeA, nodeE, 7));
            controller.edges.Add(new Edge(nodeF, nodeG, 7));
            controller.edges.Add(new Edge(nodeG, nodeF, 6));

            // Create lists of nodes in possible routes from a start to end node.
            // First we'll test this for shortest paths.
            List<Route> listACshort = new List<Route>();
            List<Route> listCCshort = new List<Route>();
            List<Route> listDGshort = new List<Route>();

            // False because for shortest paths we don't allow node revisiting.
            // 0 and 0 because we're not considering a maximum number of stops,
            // nor a maximum distance.
            listACshort = controller.GetAllPaths(nodeA, nodeC, false, true, true, 0, 0); // valid
            listCCshort = controller.GetAllPaths(nodeC, nodeC, false, true, true, 0, 0); // valid
            listDGshort = controller.GetAllPaths(nodeD, nodeG, false, true, true, 0, 0); //invalid

            // Check the amount of possible routes returned is correct.
            Assert.AreEqual(controller.GetShortestPath(listACshort), 9);
            Assert.AreEqual(controller.GetShortestPath(listCCshort), 9);
            Assert.AreEqual(controller.GetShortestPath(listDGshort), int.MaxValue);
        }
    }
}
