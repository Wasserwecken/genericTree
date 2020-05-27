using GenericTree;
using GenericTree.Presets;
using GenericVector;
using NUnit.Framework;
using static GenericTree.Presets.FreeTree.IntersectionType;

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

            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(1f, 0f), volume));
            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(-1f, 0f), volume));
            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(0f, 1f), volume));
            Assert.IsTrue(FreeTree.IntersectionTest.PointBox(new Vector(0f, -1f), volume));

            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(4f, 4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(4f, -4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(-4f, 4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(-4f, -4f), volume));

            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(4f, 0f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(-4f, 0f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(0f, 4f), volume));
            Assert.IsFalse(FreeTree.IntersectionTest.PointBox(new Vector(0f, -4f), volume));
        }

        [Test]
        public void BoxBox()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(2f, 2f));
            Box box;


            box = new Box(new Vector(0f, 0f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));


            box = new Box(new Vector(1f, 1f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(1f, -1f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(-1f, 1f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(-1f, -1f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));


            box = new Box(new Vector(1f, 0f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(-1f, 0f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(0f, -1f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(0f, 1f), new Vector(1f, 1f));
            Assert.IsTrue(FreeTree.IntersectionTest.BoxBox(box, volume));


            box = new Box(new Vector(2f, 2f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(2f, -2f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(-2f, 2f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(-2f, -2f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));


            box = new Box(new Vector(2f, 0f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(-2f, 0f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(0f, 2f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));

            box = new Box(new Vector(0f, -2f), new Vector(1f, 1f));
            Assert.IsFalse(FreeTree.IntersectionTest.BoxBox(box, volume));
        }

        [Test]
        public void BoxSphere()
        {
            var volume = new Volume<Vector>(new Vector(0f, 0f), new Vector(1.5f, 1.5f));
            Sphere sphere;

            sphere = new Sphere(new Vector(0f, 0f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));


            sphere = new Sphere(new Vector(1f, 1f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(1f, -1f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(-1f, 1f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(-1f, -1f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));


            sphere = new Sphere(new Vector(1f, 0f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(-1f, 0f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(0f, 1f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(0f, -1f), 1f);
            Assert.IsTrue(FreeTree.IntersectionTest.SphereBox(sphere, volume));


            sphere = new Sphere(new Vector(2f, 2f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(2f, -2f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(-2f, 2f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(-2f, -2f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));


            sphere = new Sphere(new Vector(2f, 0f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(-2f, 0f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(0f, 2f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));

            sphere = new Sphere(new Vector(0f, -2f), 1f);
            Assert.IsFalse(FreeTree.IntersectionTest.SphereBox(sphere, volume));
        }
    }
}
