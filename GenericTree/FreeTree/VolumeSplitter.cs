using GenericTree.Common;
using GenericVector;
using System;

namespace GenericTree.FreeTree
{
    public static class VolumeSplitter
    {
        public static Volume<Vector>[] SplitUniform(Volume<Vector> volume)
        {
            var dimensions = volume.origin.Dimensions;
            var splits = (int)Math.Pow(2, dimensions);

            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            var newVolumes = new Volume<Vector>[splits];
            for (int split = 0; split < splits; split++)
            {
                var offsetVector = Vector.ForEachAxis(offset, (i, axis) => split / Math.Pow(2, i) % 2 >= 1 ? -axis : axis);
                newVolumes[split] = new Volume<Vector>(volume.origin + offsetVector, splitSize);
            }

            return newVolumes;
        }
    }
}
