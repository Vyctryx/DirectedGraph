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
    public class RouteTest
    {
        [TestMethod]
        // Tests that deep copy constructor makes an exact copy.
        public void TestRouteCopyConstructor()
        {
            FatController controller = new FatController();
            Route route = new Route();

            route.routeNodes.Add(controller.StrToNode("A"));
            route.routeNodes.Add(controller.StrToNode("B"));
            route.routeNodes.Add(controller.StrToNode("C"));
            route.routeDistance = 13;

            Route copyRoute = new Route(route);

            Assert.AreEqual(route.routeDistance, copyRoute.routeDistance);
            CollectionAssert.AreEqual(route.routeNodes, copyRoute.routeNodes);
        }

        [TestMethod]
        // Test that our check to see if a route contains a specified node
        // node returns true or false as appropriate.
        public void TestRouteContains()
        {
            FatController controller = new FatController();
            Route route = new Route();

            route.routeNodes.Add(controller.StrToNode("A"));
            route.routeNodes.Add(controller.StrToNode("B"));
            route.routeNodes.Add(controller.StrToNode("C"));
            route.routeDistance = 13;

            Node nodeA = new Node("A");
            Node nodeD = new Node("D");

            bool containsA = route.Contains(nodeA);
            bool containsB = route.Contains(nodeD);

            Assert.IsTrue(containsA);
            Assert.IsFalse(containsB);
        }

        [TestMethod]
        // Test that our constructor method works.
        public void TestRouteConstructor()
        {
            Route route = new Route();

            Assert.IsNotNull(route);
        }
    }
}
