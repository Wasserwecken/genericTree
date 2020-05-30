using GenericTree.Common;
using System.Numerics;

namespace Tests
{
    public class TreeTests
    {
        public class TestTree : Tree<Vector2>
        {
            public TestTree() : base(new Vector2(0f, 0f), new Vector2(4f, 4f), 5, 1) { }

            protected override Volume<Vector2>[] VolumeSplit(Volume<Vector2> volume)
            {
                var splitSize = volume.size / 2f;
                var offset = splitSize / 2f;

                return new Volume<Vector2>[4]
                {
                    new Volume<Vector2>(volume.origin + offset * new Vector2(1, -1), splitSize),
                    new Volume<Vector2>(volume.origin + offset * new Vector2(1, 1), splitSize),
                    new Volume<Vector2>(volume.origin + offset * new Vector2(-1, -1), splitSize),
                    new Volume<Vector2>(volume.origin + offset * new Vector2(-1, 1), splitSize),
                };
            }
        }
    }
}
