using GenericTree.Common;
using GenericTree.FreeTree;
using GenericVector;

namespace Tests
{
    public class FreeTreeLeafSphere : ILeaf<Vector>
    {
        public Sphere boundingSphere;

        public FreeTreeLeafSphere(Vector origin, float radius)
        {
            boundingSphere = new Sphere(origin, radius);
        }

        public bool IntersectionTest(Volume<Vector> volume)
            => boundingSphere.TestIntersection(volume);
    }
}
