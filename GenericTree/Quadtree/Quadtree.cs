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


        public HashSet<ILeaf<Vector2>> FindByPoint(Vector2 point)
            => FindBy(point, Point.TestIntersection);


        public HashSet<ILeaf<Vector2>> FindByBox(Vector2 origin, Vector2 size)
            => FindByBox(new Box(origin, size));

        public HashSet<ILeaf<Vector2>> FindByBox(Box box)
            => FindBy(box, Box.TestIntersection);


        public HashSet<ILeaf<Vector2>> FindByCircle(Vector2 origin, float radius)
            => FindByCircle(new Circle(origin, radius));

        public HashSet<ILeaf<Vector2>> FindByCircle(Circle circle)
            => FindBy(circle, Circle.TestIntersection);


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
