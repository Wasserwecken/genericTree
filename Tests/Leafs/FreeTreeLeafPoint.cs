using GenericTree.Common;
using GenericTree.FreeTree;
using GenericVector;

namespace Tests
{
    public class FreeTreeLeafPoint : ILeaf<Vector>
    {
        public Vector position;

        public FreeTreeLeafPoint(Vector position)
        {
            this.position = position;
        }

        public bool IntersectionTest(Volume<Vector> volume)
            => Point.TestIntersection(position, volume);
    }
}
