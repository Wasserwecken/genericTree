using GenericTree.Common;
using System.Collections.Generic;
using System.Numerics;

namespace GenericTree.Octree
{
    public class Octree : RootNode<Vector3>
    {
        public Octree(Vector3 origin, Vector3 size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<Vector3>(origin, size), maxDepth, maxLeafsPerNode) { }

        public Octree(Volume<Vector3> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }


        public HashSet<ILeaf<Vector3>> FindByPoint(Vector3 point)
            => FindBy(point, Point.TestIntersection);


        public HashSet<ILeaf<Vector3>> FindByBox(Vector3 origin, Vector3 size)
            => FindByBox(new Box(origin, size));

        public HashSet<ILeaf<Vector3>> FindByBox(Box box)
            => FindBy(box, Box.TestIntersection);


        public HashSet<ILeaf<Vector3>> FindBySphere(Vector3 origin, float radius)
            => FindBySphere(new Sphere(origin, radius));

        public HashSet<ILeaf<Vector3>> FindBySphere(Sphere sphere)
            => FindBy(sphere, Sphere.TestIntersection);


        protected internal override Volume<Vector3>[] SplitVolume(Volume<Vector3> volume)
            => VolumeSplitter.SplitUniform(volume);
    }
}
