using GenericVector;

namespace GenericTree.Presets
{
    public class GenericTree : Tree<Vector>
    {
        public GenericTree(Volume<Vector> startVolume, int maxDepth, int maxLeafsPerNode)
            : base(startVolume, maxDepth, maxLeafsPerNode, VolumeSplit)
        { }

        private static Volume<Vector>[] VolumeSplit(Volume<Vector> volume)
        {
            return new Volume<Vector>[1];
        }
    }
}
