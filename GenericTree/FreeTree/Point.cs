using GenericTree.Common;
using GenericVector;

namespace GenericTree.FreeTree
{
    public struct Point
    {
        public readonly Vector position;

        public Point(Vector position)
        {
            this.position = position;
        }


        public bool TestIntersection(Volume<Vector> volume)
            => TestIntersection(position, volume);

        public static bool TestIntersection(Point point, Volume<Vector> volume)
            => TestIntersection(point.position, volume);

        public static bool TestIntersection(Vector position, Volume<Vector> volume)
        {
            var delta = volume.size / 2f;
            var minDimension = Vector.MinDimension(position, volume.origin, volume.size);

            for (int i = 0; i < minDimension; i++)
                if (position[i] < volume.origin[i] - delta[i] || position[i] > volume.origin[i] + delta[i])
                    return false;

            return true;
        }
    }
}
