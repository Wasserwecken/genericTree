using GenericTree;
using GenericTree.Presets;
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
        {
            return FreeTree.IntersectionTest.PointBox(position, volume);
        }
    }
}
