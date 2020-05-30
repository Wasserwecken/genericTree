using GenericTree.Common;
using GenericVector;
using System.Collections.Generic;

namespace GenericTree.FreeTree
{
    public class FreeTree : RootNode<Vector>
    {
        public FreeTree(Vector origin, Vector size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<Vector>(origin, size), maxDepth, maxLeafsPerNode) { }

        public FreeTree(Volume<Vector> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }


        public HashSet<ILeaf<Vector>> FindByPoint(Vector point)
            => FindBy(point, Point.TestIntersection);


        public HashSet<ILeaf<Vector>> FindByBox(Vector origin, Vector size)
            => FindByBox(new Box(origin, size));

        public HashSet<ILeaf<Vector>> FindByBox(Box box)
            => FindBy(box, Box.TestIntersection);


        public HashSet<ILeaf<Vector>> FindBySphere(Vector origin, float radius)
            => FindBySphere(new Sphere(origin, radius));

        public HashSet<ILeaf<Vector>> FindBySphere(Sphere sphere)
            => FindBy(sphere, Sphere.TestIntersection);


        protected internal override Volume<Vector>[] SplitVolume(Volume<Vector> volume)
            => VolumeSplitter.SplitUniform(volume);
    }
}
