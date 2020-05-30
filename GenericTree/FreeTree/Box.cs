using GenericTree.Common;
using GenericVector;

namespace GenericTree.FreeTree
{
    public struct Box
    {
        public readonly Vector origin;
        public readonly Vector size;
        public readonly Vector minBounds;
        public readonly Vector maxBounds;

        public Box(Vector origin, Vector size)
        {
            this.origin = origin;
            this.size = size;

            var delta = size / 2f;
            minBounds = origin - delta;
            maxBounds = origin + delta;
        }

        public bool TestIntersection(Volume<Vector> volume)
            => TestIntersection(minBounds, maxBounds, volume);

        public static bool TestIntersection(Box box, Volume<Vector> volume)
            => TestIntersection(box.minBounds, box.maxBounds, volume);

        public static bool TestIntersection(Vector minBounds, Vector maxBounds, Volume<Vector> volume)
        {
            var volumeDelta = volume.size / 2f;
            var minDimension = Vector.MinDimension(minBounds, maxBounds, volume.origin, volume.size);

            for (int i = 0; i < minDimension; i++)
                if (maxBounds[i] < volume.origin[i] - volumeDelta[i] || minBounds[i] > volume.origin[i] + volumeDelta[i])
                    return false;

            return true;
        }
    }
}
