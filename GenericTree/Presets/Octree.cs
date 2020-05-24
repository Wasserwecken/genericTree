using System.Collections.Generic;
using System.Numerics;
using static GenericTree.Presets.Octree.OverlapType;

namespace GenericTree.Presets
{
    public class Octree : Tree<Vector3>
    {
        public Octree(Volume<Vector3> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode, SplitVolume)
        { }


        public HashSet<ILeaf<Vector3>> SearchByPoint(Vector3 point)
        {
            return Search(point, OverlapTest.PointOverlap);
        }

        public HashSet<ILeaf<Vector3>> SearchByBox(Vector3 origin, Vector3 size)
        {
            return Search(new Box(origin, size), OverlapTest.BoxOverlap);
        }

        public HashSet<ILeaf<Vector3>> SearchBySphere(Vector3 origin, float radius)
        {
            return Search(new Sphere(origin, radius), OverlapTest.SphereOverlap);
        }


        private static Volume<Vector3>[] SplitVolume(Volume<Vector3> volume)
        {
            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            return new Volume<Vector3>[8]
            {
                new Volume<Vector3>(volume.origin + offset * new Vector3(1, 1, -1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(1, 1, 1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(1, -1, -1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(1, -1, 1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(-1, 1, -1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(-1, 1, 1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(-1, -1, -1), splitSize),
                new Volume<Vector3>(volume.origin + offset * new Vector3(-1, -1, 1), splitSize)
            };
        }

        //https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
        public static class OverlapTest
        {
            public static bool PointOverlap(Vector3 point, Volume<Vector3> volume)
            {
                var delta = volume.size / 2f;
                return !(point.X < volume.origin.X - delta.X) && !(point.X > volume.origin.X + delta.X) &&
                       !(point.Y < volume.origin.Y - delta.Y) && !(point.Y > volume.origin.Y + delta.Y) &&
                       !(point.Z < volume.origin.Z - delta.Z) && !(point.Z > volume.origin.Z + delta.Z)
                    ;
            }

            public static bool BoxOverlap(Box box, Volume<Vector3> volume)
            {
                var boxDelta = box.size / 2f;
                var volumeDelta = volume.size / 2f;
                return !(box.center.X + boxDelta.X < volume.origin.X - volumeDelta.X) && !(box.center.X - boxDelta.X > volume.origin.X + volumeDelta.X) &&
                       !(box.center.Y + boxDelta.Y < volume.origin.Y - volumeDelta.Y) && !(box.center.Y - boxDelta.Y > volume.origin.Y + volumeDelta.Y) &&
                       !(box.center.Z + boxDelta.Z < volume.origin.Z - volumeDelta.Z) && !(box.center.Z - boxDelta.Z > volume.origin.Z + volumeDelta.Z)
                    ;
            }

            public static bool SphereOverlap(Sphere sphere, Volume<Vector3> volume)
            {
                var volumeDelta = volume.size / 2f;
                var nearest = Vector3.Max(volume.origin - volumeDelta, Vector3.Min(volume.origin + volumeDelta, sphere.center));
                var distanceSqr = Vector3.DistanceSquared(nearest, sphere.center);

                return distanceSqr < sphere.radius * sphere.radius;
            }
        }

        public static class OverlapType
        {
            public struct Box
            {
                public Vector3 center;
                public Vector3 size;

                public Box(Vector3 center, Vector3 size)
                {
                    this.center = center;
                    this.size = size;
                }
            }

            public struct Sphere
            {
                public Vector3 center;
                public float radius;

                public Sphere(Vector3 center, float radius)
                {
                    this.center = center;
                    this.radius = radius;
                }
            }
        }
    }
}
