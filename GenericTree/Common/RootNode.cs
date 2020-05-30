using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("Depth: {Depth}, Leafs: {LeafCount}, Volume: {Volume}")]
    public abstract class RootNode<T> : Node<T>
    {
        public Volume<T> Volume => volume;
        public int LeafCount => leafCount;
        public int Depth => CheckDepth();


        public RootNode(T origin, T size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<T>(origin, size), maxDepth, maxLeafsPerNode) { }

        public RootNode(Volume<T> startVolume, int maxDepth, int maxLeafsPerNode)
        {
            if (maxDepth < 0)
                throw new ArgumentException("Negative depth is invalid");

            if (maxLeafsPerNode <= 0)
                throw new ArgumentException("At least one leaf per node is required");

            SetContext(startVolume, 0, maxDepth, maxLeafsPerNode, SplitVolume);
        }


        public override bool Remove(ILeaf<T> leaf)
        {
            var result = base.Remove(leaf);
            if (result) TryMerge();

            return result;
        }

        public virtual HashSet<ILeaf<T>> FindBy<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersectionTest)
        {
            var result = new HashSet<ILeaf<T>>();
            FindBy(searchType, result, intersectionTest);

            return result;
        }


        protected internal abstract Volume<T>[] SplitVolume(Volume<T> volume);
    }
}
