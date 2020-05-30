using GenericTree.Common;
using System.Collections.Generic;
using System.Numerics;

namespace GenericTree.Quadtree
{
    public class Quadtree : RootNode<Vector2>
    {
        public Quadtree(Vector2 origin, Vector2 size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<Vector2>(origin, size), maxDepth, maxLeafsPerNode) { }

        public Quadtree(Volume<Vector2> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }


        public HashSet<ILeaf<Vector2>> SearchByPoint(Vector2 point)
            => Find(point, Point.TestIntersection);


        public HashSet<ILeaf<Vector2>> SearchByBox(Vector2 origin, Vector2 size)
            => SearchByBox(new Box(origin, size));

        public HashSet<ILeaf<Vector2>> SearchByBox(Box box)
            => Find(box, Box.TestIntersection);


        public HashSet<ILeaf<Vector2>> SearchByCircle(Vector2 origin, float radius)
            => SearchByCircle(new Sphere(origin, radius));

        public HashSet<ILeaf<Vector2>> SearchByCircle(Sphere sphere)
            => Find(sphere, Sphere.TestIntersection);


        protected internal override Volume<Vector2>[] VolumeSplit(Volume<Vector2> volume)
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
    }
}
