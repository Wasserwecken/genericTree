using System;
using System.Collections.Generic;

namespace GenericTree
{
    public class Tree<T>
    {
        internal readonly int maxDepth;
        internal readonly int maxLeafsPerNode;
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

        public bool Add(ILeaf<T> leaf)
        {
            return rootNode.Add(leaf);
        }

        public bool Remove(ILeaf<T> leaf)
        {
            var result = rootNode.Remove(leaf);
            if (result) rootNode.TryMerge();
            return result;
        }

        public List<Volume<T>> ProvideVolumes()
        {
            var result = new List<Volume<T>>();
            rootNode.ProvideVolumes(result);
            return result;
        }

        public HashSet<ILeaf<T>> Search<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersection)
        {
            var result = new HashSet<ILeaf<T>>();
            rootNode.Find(searchType, result, intersection);
            return result;
        }


        internal Node<T> ProvideNode()
        {
            if (unusedNodes.Count <= 0)
                return new Node<T>(this);
            else
                return unusedNodes.Pop();
        }

        internal void ReturnNodes(List<Node<T>> nodes)
        {
            foreach (var node in nodes)
                unusedNodes.Push(node.Reset());
        }

        internal void ReturnNode(Node<T> node)
        {
            unusedNodes.Push(node.Reset());
        }
    }
}
