using GenericVector;
using System;
using static GenericTree.Presets.FreeTree.IntersectionType;

namespace GenericTree.Presets
{
    public class FreeTree : Tree<Vector>
    {
        public FreeTree(Volume<Vector> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }

        protected internal override Volume<Vector>[] VolumeSplit(Volume<Vector> volume)
        {
            var dimensions = volume.origin.Dimensions;
            var splits = (int)Math.Pow(2, dimensions);

            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            var newVolumes = new Volume<Vector>[splits];
            for(int split = 0; split < splits; split++)
            {
                var offsetVector = Vector.ForEachAxis(new Vector(dimensions), (i, axis) => split % (int)Math.Pow(2, i) > 0 ? offset[i] : -offset[i]);
                newVolumes[split] = new Volume<Vector>(volume.origin + offsetVector, splitSize);
            }

            return newVolumes;
        }

        public struct IntersectionTest
        {
            public static bool PointBox(Vector point, Volume<Vector> volume)
            {
                var delta = volume.size / 2f;

                for (int i = 0; i < point.Dimensions; i++)
                    if (point[i] < volume.origin[i] - delta[i] || point[i] > volume.origin[i] + delta[i])
                        return false;

                return true;
            }

            public static bool BoxBox(Box box, Volume<Vector> volume)
            {
                var boxDelta = box.size / 2f;
                var volumeDelta = volume.size / 2f;

                for (int i = 0; i < box.origin.Dimensions; i++)
                    if (box.origin[i] + boxDelta[i] < volume.origin[i] - volumeDelta[i] || box.origin[i] - boxDelta[i] > volume.origin[i] + volumeDelta[i])
                        return false;

                return true;
            }

            public static bool SphereBox(Sphere sphere, Volume<Vector> volume)
            {
                var volumeDelta = volume.size / 2f;
                var nearest = Vector.Max(volume.origin - volumeDelta, Vector.Min(volume.origin + volumeDelta, sphere.origin));
                var distanceSqr = Vector.DistanceSquared(nearest, sphere.origin);

                return distanceSqr < sphere.radius * sphere.radius;
            }
        }

        public struct IntersectionType
        {
            public struct Box
            {
                public readonly Vector origin;
                public readonly Vector size;

                public Box(Vector origin, Vector size)
                {
                    this.origin = origin;
                    this.size = size;
                }
            }

            public struct Sphere
            {
                public Vector origin;
                public float radius;

                public Sphere(Vector origin, float radius)
                {
                    this.origin = origin;
                    this.radius = radius;
                }
            }
        }
    }
}
