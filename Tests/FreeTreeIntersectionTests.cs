using GenericTree;
using GenericTree.Presets;
using GenericVector;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FreeTreeIntersectionTests
    {
        [Test]
        public void PointBox()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(4f, 4f));

            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(0f, 0f), volume));

            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(1f, 1f), volume));
            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(1f, -1f), volume));
            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(-1f, 1f), volume));
            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(-1f, -1f), volume));

            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(4f, 4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(4f, -4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(-4f, 4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(-4f, -4f), volume));
        }
    }
}
