using GenericTree;
using GenericTree.Presets;
using GenericVector;

namespace Tests
{
    public class FreeTreeLeafBox : ILeaf<Vector>
    {
        public Vector position;
        public Vector size;

        public FreeTreeLeafBox(Vector position, Vector size)
        {
            this.position = position;
            this.size = size;
        }

        public bool IntersectionTest(Volume<Vector> volume)
        {
            return FreeTree.IntersectionTest.PointBox(position, volume);
        }
    }
}
