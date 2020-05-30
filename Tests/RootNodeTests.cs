using GenericTree.Common;
using GenericTree.FreeTree;
using GenericVector;
using NUnit.Framework;

namespace Tests
{
    public class RootNodeTests
    {
        private TestTree tree;

        [SetUp]
        public void Setup()
        {
            tree = new TestTree();
        }


        public class TestTree : FreeTree
        {
            public TestTree() : base(new Vector(0f, 0f), new Vector(4f, 4f), 2, 1) { }
        }
    }
}
