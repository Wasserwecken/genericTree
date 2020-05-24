using System.Collections.Generic;
using System.Numerics;
using static GenericTree.Presets.Octree.IntersectionType;

namespace GenericTree.Presets
{
    public class Octree : Tree<Vector3>
    {
        public Octree(Volume<Vector3> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode, SplitVolume)
        { }


        public HashSet<ILeaf<Vector3>> SearchByPoint(Vector3 point)
        {
            return Search(point, IntersectionTest.PointBox);
        }

        public HashSet<ILeaf<Vector3>> SearchByBox(Vector3 origin, Vector3 size)
        {
            return Search(new Box(origin, size), IntersectionTest.BoxBox);
        }

        public HashSet<ILeaf<Vector3>> SearchBySphere(Vector3 origin, float radius)
        {
            return Search(new Sphere(origin, radius), IntersectionTest.SphereBox);
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
        public static class IntersectionTest
        {
            public static bool PointBox(Vector3 point, Volume<Vector3> volume)
            {
                var delta = volume.size / 2f;
                return !(point.X < volume.origin.X - delta.X) && !(point.X > volume.origin.X + delta.X) &&
                       !(point.Y < volume.origin.Y - delta.Y) && !(point.Y > volume.origin.Y + delta.Y) &&
                       !(point.Z < volume.origin.Z - delta.Z) && !(point.Z > volume.origin.Z + delta.Z)
                    ;
            }

            public static bool BoxBox(Box box, Volume<Vector3> volume)
            {
                var boxDelta = box.size / 2f;
                var volumeDelta = volume.size / 2f;
                return !(box.origin.X + boxDelta.X < volume.origin.X - volumeDelta.X) && !(box.origin.X - boxDelta.X > volume.origin.X + volumeDelta.X) &&
                       !(box.origin.Y + boxDelta.Y < volume.origin.Y - volumeDelta.Y) && !(box.origin.Y - boxDelta.Y > volume.origin.Y + volumeDelta.Y) &&
                       !(box.origin.Z + boxDelta.Z < volume.origin.Z - volumeDelta.Z) && !(box.origin.Z - boxDelta.Z > volume.origin.Z + volumeDelta.Z)
                    ;
            }

            public static bool SphereBox(Sphere sphere, Volume<Vector3> volume)
            {
                var volumeDelta = volume.size / 2f;
                var nearest = Vector3.Max(volume.origin - volumeDelta, Vector3.Min(volume.origin + volumeDelta, sphere.origin));
                var distanceSqr = Vector3.DistanceSquared(nearest, sphere.origin);

                return distanceSqr < sphere.radius * sphere.radius;
            }
        }

        public static class IntersectionType
        {
            public struct Box
            {
                public Vector3 origin;
                public Vector3 size;

                public Box(Vector3 origin, Vector3 size)
                {
                    this.origin = origin;
                    this.size = size;
                }
            }

            public struct Sphere
            {
                public Vector3 origin;
                public float radius;

                public Sphere(Vector3 origin, float radius)
                {
                    this.origin = origin;
                    this.radius = radius;
                }
            }
        }
    }
}
