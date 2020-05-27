using GenericTree;
using GenericTree.Presets;
using GenericVector;

namespace Tests
{
    public class FreeTreeLeafSphere : ILeaf<Vector>
    {
        public Vector position;
        public float radius;

        public FreeTreeLeafSphere(Vector position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public bool IntersectionTest(Volume<Vector> volume)
        {
            return FreeTree.IntersectionTest.PointBox(position, volume);
        }
    }
}
