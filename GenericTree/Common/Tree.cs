using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("LeafCount: {LeafCount}")]
    public abstract class Tree<T>
    {
        public readonly int maxDepth;
        public readonly int maxLeafsPerNode;
        public Volume<T> TreeVolume => rootNode.Volume;

        public int LeafCount => rootNode.LeafCount;

        protected readonly Stack<Node<T>> unusedNodes;
        protected readonly Node<T> rootNode;


        public Tree(T origin, T size, int maxDepth, int maxLeafsPerNode)
            : this(new Volume<T>(origin, size), maxDepth, maxLeafsPerNode) { }

        public Tree(Volume<T> startVolume, int maxDepth, int maxLeafsPerNode)
        {
            this.maxDepth = maxDepth;
            this.maxLeafsPerNode = maxLeafsPerNode;

            unusedNodes = new Stack<Node<T>>();
            rootNode = ProvideNode().SetContext(startVolume, 0);
        }


        public virtual bool Add(ILeaf<T> leaf)
            => rootNode.Add(leaf);

        public virtual bool Remove(ILeaf<T> leaf)
        {
            var result = rootNode.Remove(leaf);
            if (result) rootNode.TryMerge();
            return result;
        }

        public virtual HashSet<ILeaf<T>> Find<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersectionTest)
        {
            var result = new HashSet<ILeaf<T>>();
            rootNode.Find(searchType, result, intersectionTest);
            return result;
        }


        public virtual Node<T> ProvideNode()
        {
            if (unusedNodes.Count > 0)
                return unusedNodes.Pop();
            else
                return new Node<T>(this);
        }

        public virtual void ReturnNode(Node<T> node)
        {
            unusedNodes.Push(node.Reset());
        }

        protected internal abstract Volume<T>[] VolumeSplit(Volume<T> volume);
    }
}
