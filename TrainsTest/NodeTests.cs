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
    public class NodeTests
    {
        [TestMethod]
        // Test that constructor method works.
        public void TestNodeConstructor()
        {
            Node node = new Node();

            Assert.IsNotNull(node);
        }

        [TestMethod]
        // Test that copy constructor method works.
        public void TestNodeCopyConstructor()
        {
            Node node = new Node("A");

            Assert.IsNotNull(node);
            Assert.AreEqual(node.Name(), "A");
        }

        [TestMethod]
        // Test that Node Name method returns the node name.
        public void TestNodeName()
        {
            Node node = new Node("A");
            string nodeName = node.Name();

            Assert.AreEqual(nodeName, "A");
            Assert.AreEqual(nodeName, node.Name());
        }
    }
}
