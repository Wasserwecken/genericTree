using System.Collections.Generic;
using System.Numerics;

namespace GenericTree.Presets
{
    public class Octree : Tree<Vector3>
    {
        public Octree(TreeSettings<Vector3> settings) : base(settings, SplitVolume) { }


        public List<ILeaf<Vector3>> SearchByBox(Vector3 center, Vector3 size)
        {
            return Search(new Box(center, size), Overlaps.BoxOverlap);
        }

        public List<ILeaf<Vector3>> SearchBySphere(Vector3 center, float radius)
        {
            return Search(new Sphere(center, radius), Overlaps.SphereOverlap);
        }


        private static Volume<Vector3>[] SplitVolume(Volume<Vector3> volume)
        {
            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            return new Volume<Vector3>[8]
            {
                new Volume<Vector3>(volume.center + new Vector3(offset, offset, -offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(offset, offset, offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(offset, -offset, -offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(offset, -offset, offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(-offset, offset, -offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(-offset, offset, offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(-offset, -offset, -offset), splitSize),
                new Volume<Vector3>(volume.center + new Vector3(-offset, -offset, offset), splitSize)
            };
        }

        //https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
        public class Overlaps
        {
            public static bool PointOverlap(Vector3 point, Volume<Vector3> volume)
            {
                var delta = volume.size / 2f;
                return !(point.X < volume.center.X - delta) && !(point.X > volume.center.X + delta) &&
                       !(point.Y < volume.center.Y - delta) && !(point.Y > volume.center.Y + delta) &&
                       !(point.Z < volume.center.Z - delta) && !(point.Z > volume.center.Z + delta)
                    ;
            }

            public static bool BoxOverlap(Box box, Volume<Vector3> volume)
            {
                var boxDelta = box.size / 2f;
                var volumeDelta = volume.size / 2f;
                return !(box.center.X + boxDelta.X < volume.center.X - volumeDelta) && !(box.center.X - boxDelta.X > volume.center.X + volumeDelta) &&
                       !(box.center.Y + boxDelta.Y < volume.center.Y - volumeDelta) && !(box.center.Y - boxDelta.Y > volume.center.Y + volumeDelta) &&
                       !(box.center.Z + boxDelta.Z < volume.center.Z - volumeDelta) && !(box.center.Z - boxDelta.Z > volume.center.Z + volumeDelta)
                    ;
            }

            public static bool SphereOverlap(Sphere sphere, Volume<Vector3> volume)
            {
                var volumeDelta = new Vector3(volume.size / 2f);
                var nearest = Vector3.Max(volume.center - volumeDelta, Vector3.Min(volume.center + volumeDelta, sphere.center));
                var distanceSqr = Vector3.DistanceSquared(nearest, sphere.center);

                return distanceSqr < sphere.radius * sphere.radius;
            }
        }
    }
}
