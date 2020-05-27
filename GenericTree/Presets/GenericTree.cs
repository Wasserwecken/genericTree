using GenericVector;

namespace GenericTree.Presets
{
    public class GenericTree : Tree<Vector>
    {
        public GenericTree(Volume<Vector> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode) { }

        protected internal override Volume<Vector>[] VolumeSplit(Volume<Vector> volume)
        {
            return new Volume<Vector>[0];
        }
    }
}
