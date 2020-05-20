using System;
using GenericVector;

namespace GenericTree.Presets
{
    public class GenericTree : Tree<GVector>
    {
        public GenericTree(TreeSettings<GVector> settings) : base(settings, SplitVolume) { }

        private static Volume<GVector>[] SplitVolume(Volume<GVector> volume)
        {
            var dimensions = volume.center.Dimensions;
            var splitCount = (int)Math.Pow(2, dimensions);

            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            var splits = new Volume<GVector>[splitCount];
            for(int s = 1; s <= splitCount; s++)
            {
                var offsetVector = new GVector(dimensions).ForEachAxis((i, axis) => s % (int)Math.Pow(2, i) > 0 ? offset : -offset);
                splits[s] = new Volume<GVector>(volume.center + offsetVector[s], splitSize);
            }

            return splits;
        }
    }
}

