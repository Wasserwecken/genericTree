using GenericTree.Common;
using GenericVector;
using System.Collections.Generic;

namespace GenericTree.FreeTree
{
    public class FreeTree : RootBase<Vector, Node<Vector>>
    {
        public FreeTree(Vector origin, Vector size, int maxDepth, int maxLeafsPerNode)
            : base(origin, size, maxDepth, maxLeafsPerNode) { }


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


        protected override Node<Vector> CreateRootNode()
            => new Node<Vector>(Volume, 0, MaxDepth, MaxLeafsPerNode, VolumeSplitter.SplitUniform);
    }
}
