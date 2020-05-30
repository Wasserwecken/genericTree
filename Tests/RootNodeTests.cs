using GenericTree.Common;
using GenericTree.FreeTree;
using GenericVector;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class RootNodeTests
    {
        private TestTree tree;

        [SetUp]
        public void Setup()
        {
            tree = new TestTree();
        }

        [Test]
        public void Add()
        {
            Assert.IsFalse(tree.Add(new FreeTreeLeafPoint(new Vector(5f, 5f))));
            Assert.AreEqual(0, tree.LeafCount);
            Assert.AreEqual(0, tree.Depth);

            Assert.IsTrue(tree.Add(new FreeTreeLeafPoint(new Vector(0.5f, 0.5f))));
            Assert.AreEqual(1, tree.LeafCount);
            Assert.AreEqual(0, tree.Depth);

            Assert.IsTrue(tree.Add(new FreeTreeLeafBox(new Vector(-1f, 1f), new Vector(3f, 3f))));
            Assert.AreEqual(2, tree.LeafCount);
            Assert.AreEqual(0, tree.Depth);

            Assert.IsTrue(tree.Add(new FreeTreeLeafSphere(new Vector(1f, -1f), 1f)));
            Assert.AreEqual(3, tree.LeafCount);
            Assert.AreEqual(1, tree.Depth);

            var newItem = new FreeTreeLeafPoint(new Vector(1.5f, 1.5f));
            Assert.IsTrue(tree.Add(newItem));
            Assert.AreEqual(4, tree.LeafCount);
            Assert.AreEqual(2, tree.Depth);

            Assert.IsFalse(tree.Add(newItem));
            Assert.AreEqual(4, tree.LeafCount);
            Assert.AreEqual(2, tree.Depth);
        }

        [Test]
        public void Remove()
        {
            tree.Add(new FreeTreeLeafPoint(new Vector(1f, 1f)));
            tree.Add(new FreeTreeLeafPoint(new Vector(1f, 1f)));

            var item = new FreeTreeLeafPoint(new Vector(1f, 1f));
            tree.Add(item);

            Assert.IsFalse(tree.Remove(new FreeTreeLeafPoint(new Vector(1f, 1f))));
            Assert.AreEqual(3, tree.LeafCount);
            Assert.AreEqual(2, tree.Depth);

            Assert.IsTrue(tree.Remove(item));
            Assert.AreEqual(2, tree.LeafCount);
            Assert.AreEqual(0, tree.Depth);

            Assert.IsFalse(tree.Remove(item));
            Assert.AreEqual(2, tree.LeafCount);
            Assert.AreEqual(0, tree.Depth);
        }

        [Test]
        public void FindBy()
        {
            var point = new FreeTreeLeafPoint(new Vector(1f, 1f));
            tree.Add(point);
            var box = new FreeTreeLeafBox(new Vector(-1f, 1f), new Vector(3f, 3f));
            tree.Add(box);
            var sphere = new FreeTreeLeafSphere(new Vector(1f, -1f), 0.5f);
            tree.Add(sphere);

            var result = tree.FindBySphere(new Vector(0f, 0f), 1f);

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Contains(point));
            Assert.IsTrue(result.Contains(box));
            Assert.IsTrue(result.Contains(sphere));


            result = tree.FindByBox(new Vector(0f, 1f), new Vector(2f, 0.1f));
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(point));
            Assert.IsTrue(result.Contains(box));
        }


        public class TestTree : FreeTree
        {
            public TestTree() : base(new Vector(0f, 0f), new Vector(4f, 4f), 2, 2) { }
        }
    }
}
