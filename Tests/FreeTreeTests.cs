using GenericVector;
using GenericTree.Presets;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FreeTreeTests
    {
        public FreeTree tree;

        [SetUp]
        public void Setup()
        {
            tree = new FreeTree(new Vector(0f, 0f), new Vector(4f, 4f), 4, 1);
        }

        [Test]
        public void Add()
        {
            tree.Add(new FreeTreeLeafPoint(new Vector(1f, 1f)));
            Assert.AreEqual(1, tree.LeafCount);

            tree.Add(new FreeTreeLeafPoint(new Vector(1f, -1f)));
            Assert.AreEqual(2, tree.LeafCount);

            tree.Add(new FreeTreeLeafPoint(new Vector(-1f, 1f)));
            Assert.AreEqual(3, tree.LeafCount);

            tree.Add(new FreeTreeLeafPoint(new Vector(-1f, -1f)));
            Assert.AreEqual(4, tree.LeafCount);
        }
    }
}