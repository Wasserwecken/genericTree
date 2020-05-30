using GenericTree.Common;
using GenericVector;

namespace GenericTree.FreeTree
{
    public class FreeTree : RootNode<Vector>
    {
        public FreeTree(Vector origin, Vector size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<Vector>(origin, size), maxDepth, maxLeafsPerNode) { }

        public FreeTree(Volume<Vector> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }

        protected internal override Volume<Vector>[] SplitVolume(Volume<Vector> volume)
            => VolumeSplitter.SplitUniform(volume);
    }
}
