using GenericTree.Common;
using GenericTree.FreeTree;
using GenericVector;

namespace Tests
{
    public class FreeTreeLeafBox : ILeaf<Vector>
    {
        public Box boundingBox;

        public FreeTreeLeafBox(Vector origin, Vector size)
        {
            boundingBox = new Box(origin, size);
        }

        public bool IntersectionTest(Volume<Vector> volume)
            => boundingBox.TestIntersection(volume);
    }
}
