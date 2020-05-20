using System;

namespace GenericTree.Presets
{
    public class GenericTree : Tree<GenericVector>
    {
        public GenericTree(TreeSettings<GenericVector> settings) : base(settings, SplitVolume)
        {

        }

        private static Volume<GenericVector>[] SplitVolume(Volume<GenericVector> volume)
        {
            var dimensions = volume.center.Dimensions;
            var splitCount = (int)Math.Pow(2, dimensions);

            var splitSize = volume.size / 2f;
            var offset = splitSize / 2f;

            var splits = new Volume<GenericVector>[splitCount];
            for(int s = 1; s <= splitCount; s++)
            {
                var offsetVector = new GenericVector(dimensions);
                for (int a = 0; a < dimensions; a++)
                    offsetVector[a] = s % (int)Math.Pow(2, a) > 0 ? offset : -offset;

                splits[s] = new Volume<GenericVector>(volume.center + offsetVector[s], splitSize);
            }

            return splits;
        }
    }
}

