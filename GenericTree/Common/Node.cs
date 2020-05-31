using System;
using System.Diagnostics;

namespace GenericTree.Common
{
    public class Node<T> : NodeBase<T, Node<T>>
    {
        public Node() : base() { }

        public Node(Volume<T> volume, int depth, int maxDepth, int maxLeafsPerNode, Func<Volume<T>, Volume<T>[]> volumeSplit)
            : base(volume, depth, maxDepth, maxLeafsPerNode, volumeSplit) { }
    }
}
