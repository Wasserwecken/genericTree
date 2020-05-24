using System.Collections.Generic;
using System.Numerics;
using static GenericTree.Presets.Quadtree.OverlapType;

namespace GenericTree.Presets
{
    public class Quadtree : Tree<Vector2>
    {
        public Quadtree(TreeSettings<Vector2> settings) : base(settings, SplitVolume) { }


        public HashSet<ILeaf<Vector2>> SearchByPoint(Vector2 point)
        {
            return Search(point, OverlapTest.PointOverlap);
        }

        public HashSet<ILeaf<Vector2>> SearchByBox(Vector2 center, Vector2 size)
        {
            return Search(new Box(center, size), OverlapTest.BoxOverlap);
        }

        public HashSet<ILeaf<Vector2>> SearchByCircle(Vector2 center, float radius)
        {
            return Search(new Sphere(center, radius), OverlapTest.SphereOverlap);
        }


        private static Volume<Vector2>[] SplitVolume(Volume<Vector2> volume)
        {
            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            return new Volume<Vector2>[4]
            {
                new Volume<Vector2>(volume.center + offset * new Vector2(1, -1), splitSize),
                new Volume<Vector2>(volume.center + offset * new Vector2(1, 1), splitSize),
                new Volume<Vector2>(volume.center + offset * new Vector2(-1, -1), splitSize),
                new Volume<Vector2>(volume.center + offset * new Vector2(-1, 1), splitSize),
            };
        }

        //https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
        public static class OverlapTest
        {
            public static bool PointOverlap(Vector2 point, Volume<Vector2> volume)
            {
                var delta = volume.size / 2f;
                return !(point.X < volume.center.X - delta.X) && !(point.X > volume.center.X + delta.X) &&
                       !(point.Y < volume.center.Y - delta.Y) && !(point.Y > volume.center.Y + delta.Y)
                    ;
            }

            public static bool BoxOverlap(Box box, Volume<Vector2> volume)
            {
                var boxDelta = box.size / 2f;
                var volumeDelta = volume.size / 2f;
                return !(box.center.X + boxDelta.X < volume.center.X - volumeDelta.X) && !(box.center.X - boxDelta.X > volume.center.X + volumeDelta.X) &&
                       !(box.center.Y + boxDelta.Y < volume.center.Y - volumeDelta.Y) && !(box.center.Y - boxDelta.Y > volume.center.Y + volumeDelta.Y)
                    ;
            }

            public static bool SphereOverlap(Sphere sphere, Volume<Vector2> volume)
            {
                var volumeDelta = volume.size / 2f;
                var nearest = Vector2.Max(volume.center - volumeDelta, Vector2.Min(volume.center + volumeDelta, sphere.center));
                var distanceSqr = Vector2.DistanceSquared(nearest, sphere.center);

                return distanceSqr < sphere.radius * sphere.radius;
            }
        }

        public static class OverlapType
        {
            public struct Box
            {
                public Vector2 center;
                public Vector2 size;

                public Box(Vector2 center, Vector2 size)
                {
                    this.center = center;
                    this.size = size;
                }
            }

            public struct Sphere
            {
                public Vector2 center;
                public float radius;

                public Sphere(Vector2 center, float radius)
                {
                    this.center = center;
                    this.radius = radius;
                }
            }
        }
    }
}
