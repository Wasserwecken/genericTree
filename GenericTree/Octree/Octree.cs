using GenericTree.Common;
using System.Collections.Generic;
using System.Numerics;

namespace GenericTree.Octree
{
    public class Octree : Tree<Vector3>
    {
        public Octree(Vector3 origin, Vector3 size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<Vector3>(origin, size), maxDepth, maxLeafsPerNode) { }

        public Octree(Volume<Vector3> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }


        public HashSet<ILeaf<Vector3>> SearchByPoint(Vector3 point)
            => Find(point, Point.TestIntersection);


        public HashSet<ILeaf<Vector3>> SearchByBox(Vector3 origin, Vector3 size)
            => SearchByBox(new Box(origin, size));

        public HashSet<ILeaf<Vector3>> SearchByBox(Box box)
            => Find(box, Box.TestIntersection);


        public HashSet<ILeaf<Vector3>> SearchBySphere(Vector3 origin, float radius)
            => SearchBySphere(new Sphere(origin, radius));

        public HashSet<ILeaf<Vector3>> SearchBySphere(Sphere sphere)
            => Find(sphere, Sphere.TestIntersection);


        protected internal override Volume<Vector3>[] VolumeSplit(Volume<Vector3> volume)
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
    }
}
