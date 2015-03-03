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
    public class EdgeTests
    {
        [TestMethod]
        // Test that our Edge constructor sets its variables properly.
        public void TestEdgeConstructor()
        {
            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            int dist = 4;

            Edge edge = new Edge(nodeA, nodeB, dist);

            Assert.IsNotNull(edge);
            Assert.AreEqual(edge.startNode.Name(), "A");
            Assert.AreEqual(edge.endNode.Name(), "B");
            Assert.AreEqual(edge.distance, dist);
        }

        [TestMethod]
        // Test that PrintEdge returns a correct string.
        public void TestEdgePrintEdge()
        {
            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            int dist = 4;

            Edge edge = new Edge(nodeA, nodeB, dist);

            string edgeInfo;
            edgeInfo = edge.PrintEdge();

            string expected = "A -> B, Distance: 4";

            Assert.IsNotNull(edgeInfo);
            Assert.AreEqual(edgeInfo, expected);
        }
    }
}
