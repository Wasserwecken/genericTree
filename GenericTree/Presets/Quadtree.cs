﻿using System.Collections.Generic;
using System.Numerics;
using static GenericTree.Presets.Quadtree.OverlapType;

namespace GenericTree.Presets
{
    public class Quadtree : Tree<Vector2>
    {
        public Quadtree(Volume<Vector2> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode, SplitVolume)
        { }

        public HashSet<ILeaf<Vector2>> SearchByPoint(Vector2 point)
        {
            return Search(point, OverlapTest.PointOverlap);
        }

        public HashSet<ILeaf<Vector2>> SearchByBox(Vector2 origin, Vector2 size)
        {
            return Search(new Box(origin, size), OverlapTest.BoxOverlap);
        }

        public HashSet<ILeaf<Vector2>> SearchByCircle(Vector2 origin, float radius)
        {
            return Search(new Sphere(origin, radius), OverlapTest.SphereOverlap);
        }


        private static Volume<Vector2>[] SplitVolume(Volume<Vector2> volume)
        {
            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            return new Volume<Vector2>[4]
            {
                new Volume<Vector2>(volume.origin + offset * new Vector2(1, -1), splitSize),
                new Volume<Vector2>(volume.origin + offset * new Vector2(1, 1), splitSize),
                new Volume<Vector2>(volume.origin + offset * new Vector2(-1, -1), splitSize),
                new Volume<Vector2>(volume.origin + offset * new Vector2(-1, 1), splitSize),
            };
        }

        //https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
        public static class OverlapTest
        {
            public static bool PointOverlap(Vector2 point, Volume<Vector2> volume)
            {
                var delta = volume.size / 2f;
                return !(point.X < volume.origin.X - delta.X) && !(point.X > volume.origin.X + delta.X) &&
                       !(point.Y < volume.origin.Y - delta.Y) && !(point.Y > volume.origin.Y + delta.Y)
                    ;
            }

            public static bool BoxOverlap(Box box, Volume<Vector2> volume)
            {
                var boxDelta = box.size / 2f;
                var volumeDelta = volume.size / 2f;
                return !(box.origin.X + boxDelta.X < volume.origin.X - volumeDelta.X) && !(box.origin.X - boxDelta.X > volume.origin.X + volumeDelta.X) &&
                       !(box.origin.Y + boxDelta.Y < volume.origin.Y - volumeDelta.Y) && !(box.origin.Y - boxDelta.Y > volume.origin.Y + volumeDelta.Y)
                    ;
            }

            public static bool SphereOverlap(Sphere sphere, Volume<Vector2> volume)
            {
                var volumeDelta = volume.size / 2f;
                var nearest = Vector2.Max(volume.origin - volumeDelta, Vector2.Min(volume.origin + volumeDelta, sphere.origin));
                var distanceSqr = Vector2.DistanceSquared(nearest, sphere.origin);

                return distanceSqr < sphere.radius * sphere.radius;
            }
        }

        public static class OverlapType
        {
            public struct Box
            {
                public Vector2 origin;
                public Vector2 size;

                public Box(Vector2 origin, Vector2 size)
                {
                    this.origin = origin;
                    this.size = size;
                }
            }

            public struct Sphere
            {
                public Vector2 origin;
                public float radius;

                public Sphere(Vector2 origin, float radius)
                {
                    this.origin = origin;
                    this.radius = radius;
                }
            }

            public struct Ray
            {
                public Vector2 origin;
                public Vector2 direction;
                public Vector2 directionInv;
                public float length;

                public Ray(Vector2 origin, Vector2 direction, float length)
                {
                    this.origin = origin;
                    this.direction = direction;
                    this.length = length;

                    directionInv = new Vector2(
                            1f / direction.X,
                            1f / direction.Y
                        );
                }
            }
        }
    }
}
