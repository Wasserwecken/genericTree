using GenericTree.Common;
using System.Numerics;

namespace GenericTree.Octree
{
    public struct Point
    {
        public readonly Vector3 position;

        public Point(Vector3 position)
        {
            this.position = position;
        }


        public bool TestIntersection(Volume<Vector3> volume)
            => TestIntersection(position, volume);

        public static bool TestIntersection(Point point, Volume<Vector3> volume)
            => TestIntersection(point.position, volume);

        public static bool TestIntersection(Vector3 position, Volume<Vector3> volume)
        {
            var delta = volume.size / 2f;
            return !(position.X < volume.origin.X - delta.X) && !(position.X > volume.origin.X + delta.X) &&
                   !(position.Y < volume.origin.Y - delta.Y) && !(position.Y > volume.origin.Y + delta.Y) &&
                   !(position.Z < volume.origin.Z - delta.Z) && !(position.Z > volume.origin.Z + delta.Z)
                ;
        }
    }
}
