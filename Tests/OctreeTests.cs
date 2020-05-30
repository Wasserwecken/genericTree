using GenericTree.Common;
using GenericTree.Octree;
using NUnit.Framework;
using System.Numerics;

namespace Tests
{
    [TestFixture]
    public class OctreeTests
    {
        [Test]
        public void SplitUniform()
        {
            var volume = new Volume<Vector3>(new Vector3(0f, 0f, 0f), new Vector3(4f, 4f, 4f));

            var result = VolumeSplitter.SplitUniform(volume);

            Assert.AreEqual(8, result.Length);

            Assert.AreEqual(new Vector3(1f, 1f, 1f), result[0].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[0].size);

            Assert.AreEqual(new Vector3(-1f, 1f, 1f), result[1].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[1].size);

            Assert.AreEqual(new Vector3(1f, -1f, 1f), result[2].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[2].size);

            Assert.AreEqual(new Vector3(-1f, -1f, 1f), result[3].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[3].size);

            Assert.AreEqual(new Vector3(1f, 1f, -1f), result[4].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[4].size);

            Assert.AreEqual(new Vector3(-1f, 1f, -1f), result[5].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[5].size);

            Assert.AreEqual(new Vector3(1f, -1f, -1f), result[6].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[6].size);

            Assert.AreEqual(new Vector3(-1f, -1f, -1f), result[7].origin);
            Assert.AreEqual(new Vector3(2f, 2f, 2f), result[7].size);
        }

        [Test]
        public void PointIntersection()
        {
            var volume = new Volume<Vector3>(new Vector3(0f, 0f, 0f), new Vector3(4f, 4f, 4f));

            Assert.IsTrue(Point.TestIntersection(new Vector3(0f, 0f, 0f), volume));

            Assert.IsTrue(Point.TestIntersection(new Vector3(1f, 1f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector3(-1f, 1f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector3(1f, -1f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector3(-1f, -1f, 0f), volume));

            Assert.IsTrue(Point.TestIntersection(new Vector3(1f, 0f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector3(-1f, 0f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector3(0f, 1f, 0f), volume));
            Assert.IsTrue(Point.TestIntersection(new Vector3(0f, -1f, 0f), volume));

            Assert.IsFalse(Point.TestIntersection(new Vector3(4f, 4f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector3(-4f, 4f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector3(4f, -4f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector3(-4f, -4f, 0f), volume));

            Assert.IsFalse(Point.TestIntersection(new Vector3(4f, 0f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector3(-4f, 0f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector3(0f, 4f, 0f), volume));
            Assert.IsFalse(Point.TestIntersection(new Vector3(0f, -4f, 0f), volume));
        }

        [Test]
        public void BoxIntersection()
        {
            var volume = new Volume<Vector3>(new Vector3(0f, 0f ,0f), new Vector3(2f, 2f, 2f));

            Assert.IsTrue(new Box(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(0f, 0f, 0f), new Vector3(4f, 4f, 0f)).TestIntersection(volume));

            Assert.IsTrue(new Box(new Vector3(1f, 1f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(-1f, 1f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(1f, -1f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(-1f, -1f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));

            Assert.IsTrue(new Box(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(-1f, 0f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(0f, 1f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsTrue(new Box(new Vector3(0f, -1f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));

            Assert.IsFalse(new Box(new Vector3(4f, 4f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector3(-4f, 4f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector3(4f, -4f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector3(-4f, -4f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));

            Assert.IsFalse(new Box(new Vector3(4f, 0f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector3(-4f, 0f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector3(0f, 4f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
            Assert.IsFalse(new Box(new Vector3(0f, -4f, 0f), new Vector3(1f, 1f, 0f)).TestIntersection(volume));
        }

        [Test]
        public void SphereIntersection()
        {
            var volume = new Volume<Vector3>(new Vector3(0f, 0f, 0f), new Vector3(1.5f, 1.5f, 1.5f));

            Assert.IsTrue(new Sphere(new Vector3(0f, 0f, 0f), 0.1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(0f, 0f, 0f), 4f).TestIntersection(volume));

            Assert.IsTrue(new Sphere(new Vector3(1f, 1f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(-1f, 1f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(1f, -1f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(-1f, -1f, 0f), 1f).TestIntersection(volume));

            Assert.IsTrue(new Sphere(new Vector3(1f, 0f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(-1f, 0f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(0f, 1f, 0f), 1f).TestIntersection(volume));
            Assert.IsTrue(new Sphere(new Vector3(0f, -1f, 0f), 1f).TestIntersection(volume));

            Assert.IsFalse(new Sphere(new Vector3(4f, 4f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector3(-4f, 4f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector3(4f, -4f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector3(-4f, -4f, 0f), 1f).TestIntersection(volume));

            Assert.IsFalse(new Sphere(new Vector3(4f, 0f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector3(-4f, 0f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector3(0f, 4f, 0f), 1f).TestIntersection(volume));
            Assert.IsFalse(new Sphere(new Vector3(0f, -4f, 0f), 1f).TestIntersection(volume));
        }
    }
}
