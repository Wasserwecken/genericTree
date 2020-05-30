using GenericTree.Common;
using GenericVector;

namespace GenericTree.FreeTree
{
    public struct Sphere
    {
        public readonly Vector origin;
        public readonly float radius;

        public Sphere(Vector origin, float radius)
        {
            this.origin = origin;
            this.radius = radius;
        }


        public bool TestIntersection(Volume<Vector> volume)
            => TestIntersection(origin, radius, volume);

        public static bool TestIntersection(Sphere sphere, Volume<Vector> volume)
            => TestIntersection(sphere.origin, sphere.radius, volume);

        public static bool TestIntersection(Vector origin, float radius, Volume<Vector> volume)
        {
            var volumeDelta = volume.size / 2f;

            var nearest = Vector.Max(volume.origin - volumeDelta, Vector.Min(volume.origin + volumeDelta, origin));
            var distanceSqr = Vector.DistanceSquared(nearest, origin);

            return distanceSqr < radius * radius;
        }
    }
}
