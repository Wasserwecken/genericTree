using GenericTree.Common;
using System.Numerics;

namespace GenericTree.Octree
{
    public static class VolumeSplitter
    {
        public static Volume<Vector3>[] SplitUniform(Volume<Vector3> volume)
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
