using System;
using System.Collections.Generic;

namespace GenericTree
{
    public class Tree<T>
    {
        public readonly int maxDepth;
        public readonly int maxLeafsPerNode;
        public Volume<T> TreeVolume => rootNode.Volume;

        internal readonly Func<Volume<T>, Volume<T>[]> splitVolume;
        private readonly Stack<Node<T>> unusedNodes;
        private readonly Node<T> rootNode;

        public Tree(
            Volume<T> startVolume,
            int maxDepth,
            int maxLeafsPerNode,
            Func<Volume<T>, Volume<T>[]> splitVolume)
        {
            this.maxDepth = maxDepth;
            this.maxLeafsPerNode = maxLeafsPerNode;
            this.splitVolume = splitVolume;

            unusedNodes = new Stack<Node<T>>();
            rootNode = CreateNode().Context(startVolume, 0);
        }

        public virtual bool Add(ILeaf<T> leaf)
        {
            return rootNode.Add(leaf);
        }

        public virtual bool Remove(ILeaf<T> leaf)
        {
            var result = rootNode.Remove(leaf);
            if (result) rootNode.TryMerge();
            return result;
        }

        public virtual List<Node<T>> ProvideNodes(int minDepth = 0, int maxDepth = 0)
        {
            var result = new List<Node<T>>();
            ProvideNodes(result, minDepth, maxDepth);
            return result;
        }

        public virtual void ProvideNodes(List<Node<T>> result, int minDepth = 0, int maxDepth = 0)
            => rootNode.ProvideNodes(result, minDepth, maxDepth);

        public virtual List<Volume<T>> ProvideVolumes(int minDepth = 0, int maxDepth = 0)
        {
            var result = new List<Volume<T>>();
            ProvideVolumes(result, minDepth, maxDepth);
            return result;
        }

        public virtual void ProvideVolumes(List<Volume<T>> result, int minDepth = 0, int maxDepth = 0)
            => rootNode.ProvideVolumes(result, minDepth, maxDepth);

        public virtual HashSet<ILeaf<T>> Search<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersection)
        {
            var result = new HashSet<ILeaf<T>>();
            rootNode.Find(searchType, result, intersection);
            return result;
        }


        internal virtual Node<T> CreateNode()
        {
            if (unusedNodes.Count > 0)
                return unusedNodes.Pop();
            else
                return new Node<T>(this);
        }

        internal virtual void ReturnNode(Node<T> node)
        {
            unusedNodes.Push(node.Reset());
        }
    }
}
