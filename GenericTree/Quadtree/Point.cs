using GenericTree.Common;
using System;
using System.Numerics;

namespace GenericTree.Quadtree
{
    public struct Point
    {
        public readonly Vector2 position;

        public Point(Vector2 position)
        {
            this.position = position;
        }


        public bool TestIntersection(Volume<Vector2> volume)
            => TestIntersection(position, volume);

        public static bool TestIntersection(Point point, Volume<Vector2> volume)
            => TestIntersection(point.position, volume);

        public static bool TestIntersection(Vector2 position, Volume<Vector2> volume)
        {
            var delta = volume.size / 2f;
            return !(position.X < volume.origin.X - delta.X) && !(position.X > volume.origin.X + delta.X) &&
                   !(position.Y < volume.origin.Y - delta.Y) && !(position.Y > volume.origin.Y + delta.Y)
                ;
        }
    }
}
