using GenericTree.Common;
using GenericTree.FreeTree;
using GenericVector;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FreeTreeTests
    {
        [Test]
        public void SplitUniform()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(4f, 4f));

            var result = VolumeSplitter.SplitUniform(volume);

            Assert.AreEqual(4, result.Length);

            Assert.AreEqual(new Vector(1f, 1f), result[0].origin);
            Assert.AreEqual(new Vector(2f, 2f), result[0].size);

            Assert.AreEqual(new Vector(-1f, 1f), result[1].origin);
            Assert.AreEqual(new Vector(2f, 2f), result[1].size);

            Assert.AreEqual(new Vector(1f, -1f), result[2].origin);
            Assert.AreEqual(new Vector(2f, 2f), result[2].size);

            Assert.AreEqual(new Vector(-1f, -1f), result[3].origin);
            Assert.AreEqual(new Vector(2f, 2f), result[3].size);
        }

        [Test]
        public void PointIntersection()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(4f, 4f));

            Assert.IsTrue(Point.TestIntersection(new Vector(0f, 0f), volume));

            Assert.IsTrue(Point.TestIntersection(new Vector(1f, 1f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector(-1f, 1f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector(1f, -1f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector(-1f, -1f), volume));

            Assert.IsTrue(Point.TestIntersection(new Vector(1f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector(-1f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector(0f, 1f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector(0f, -1f), volume));

            Assert.IsFalse(Point.TestIntersection(new Vector(4f, 4f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector(-4f, 4f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector(4f, -4f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector(-4f, -4f), volume));

            Assert.IsFalse(Point.TestIntersection(new Vector(4f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector(-4f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector(0f, 4f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector(0f, -4f), volume));
        }

        [Test]
        public void BoxIntersection()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(2f, 2f));

            Assert.IsTrue(new Box(new Vector(0f, 0f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(0f, 0f), new Vector(4f, 4f)).TestIntersection(volume));

            Assert.IsTrue(new Box(new Vector(1f, 1f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(-1f, 1f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(1f, -1f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(-1f, -1f), new Vector(1f, 1f)).TestIntersection(volume));

            Assert.IsTrue(new Box(new Vector(1f, 0f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(-1f, 0f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(0f, 1f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector(0f, -1f), new Vector(1f, 1f)).TestIntersection(volume));

            Assert.IsFalse(new Box(new Vector(4f, 4f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector(-4f, 4f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector(4f, -4f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector(-4f, -4f), new Vector(1f, 1f)).TestIntersection(volume));

            Assert.IsFalse(new Box(new Vector(4f, 0f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector(-4f, 0f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector(0f, 4f), new Vector(1f, 1f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector(0f, -4f), new Vector(1f, 1f)).TestIntersection(volume));
        }

        [Test]
        public void SphereIntersection()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(1.5f, 1.5f));

            Assert.IsTrue(new Sphere(new Vector(0f, 0f), 0.1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(0f, 0f), 4f).TestIntersection(volume));

            Assert.IsTrue(new Sphere(new Vector(1f, 1f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(-1f, 1f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(1f, -1f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(-1f, -1f), 1f).TestIntersection(volume));

            Assert.IsTrue(new Sphere(new Vector(1f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(-1f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(0f, 1f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector(0f, -1f), 1f).TestIntersection(volume));

            Assert.IsFalse(new Sphere(new Vector(4f, 4f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector(-4f, 4f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector(4f, -4f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector(-4f, -4f), 1f).TestIntersection(volume));

            Assert.IsFalse(new Sphere(new Vector(4f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector(-4f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector(0f, 4f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector(0f, -4f), 1f).TestIntersection(volume));
        }
    }
}
