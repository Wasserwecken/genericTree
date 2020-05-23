using System;
using System.Collections.Generic;

namespace GenericTree
{
    public class Tree<T>
    {
        internal readonly TreeSettings<T> settings;
        internal readonly Func<Volume<T>, Volume<T>[]> splitVolume;

        private readonly Stack<Node<T>> unusedNodes;
        private readonly Node<T> rootNode;

        public Tree(
            TreeSettings<T> settings,
            Func<Volume<T>, Volume<T>[]> splitVolume)
        {
            this.settings = settings;
            this.splitVolume = splitVolume;
            unusedNodes = new Stack<Node<T>>();
            rootNode = ProvideNode().Context(this, settings.volume, 1);
        }

        public bool Add(ILeaf<T> leaf)
        {
            return rootNode.Add(leaf);
        }

        public bool Remove(ILeaf<T> leaf)
        {
            return rootNode.Remove(leaf);
        }

        public List<Volume<T>> ProvideVolumes()
        {
            var result = new List<Volume<T>>();
            rootNode.ProvideVolumes(result);
            return result;
        }

        public List<ILeaf<T>> Search<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> overlap)
        {
            var result = new List<ILeaf<T>>();
            rootNode.Find(searchType, result, overlap);
            return result;
        }


        internal Node<T> ProvideNode()
        {
            if (unusedNodes.Count <= 0)
                return new Node<T>();
            else
                return unusedNodes.Pop();
        }

        internal void ReturnNodes(List<Node<T>> nodes)
        {
            foreach (var node in nodes)
                unusedNodes.Push(node.Reset());
        }
    }
}
