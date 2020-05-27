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
            rootNode = ProvideNode().Context(startVolume, 0);
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

        public virtual List<Volume<T>> ProvideVolumes()
        {
            var result = new List<Volume<T>>();
            rootNode.ProvideVolumes(result);
            return result;
        }

        public virtual HashSet<ILeaf<T>> Search<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersection)
        {
            var result = new HashSet<ILeaf<T>>();
            rootNode.Find(searchType, result, intersection);
            return result;
        }


        protected virtual Node<T> ProvideNode()
        {
            if (unusedNodes.Count <= 0)
                return new Node<T>(this);
            else
                return unusedNodes.Pop();
        }

        protected virtual void ReturnNodes(List<Node<T>> nodes)
        {
            foreach (var node in nodes)
                unusedNodes.Push(node.Reset());
        }

        protected virtual void ReturnNode(Node<T> node)
        {
            unusedNodes.Push(node.Reset());
        }
    }
}
