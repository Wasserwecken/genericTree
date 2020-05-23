using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree
{
    [DebuggerDisplay("Level: {Level};")]
    internal class Node<T>
    {
        public int Level { get; private set; }
        public Volume<T> NodeVolume { get; private set; }

        private Tree<T> tree;
        private readonly List<ILeaf<T>> leafs;
        private readonly List<Node<T>> childNodes;
        private bool HasChildren => childNodes.Count > 0;


        public Node()
        {
            childNodes = new List<Node<T>>();
            leafs = new List<ILeaf<T>>();
        }

        public Node<T> Context(
            Tree<T> tree,
            Volume<T> nodeVolume,
            int level)
        {
            this.tree = tree;
            NodeVolume = nodeVolume;
            Level = level;
            
            childNodes.Clear();
            leafs.Clear();

            return this;
        }

        public Node<T> Reset()
        {
            tree = null;
            childNodes.Clear();
            leafs.Clear();

            return this;
        }

        public bool Add(ILeaf<T> leaf)
        {
            if (leaf.CheckOverlap(NodeVolume))
            {
                if(HasChildren)
                    AddToChildren(leaf);
                else
                {
                    leafs.Add(leaf);

                    if (   leafs.Count > tree.settings.maxNodeLeafs
                        && NodeVolume.size > tree.settings.minVolumeSize * 2.0)
                        Split();
                }

                return true;
            }
            else
                return false;
        }

        public bool Remove(ILeaf<T> leaf)
        {
            if(leaf.CheckOverlap(NodeVolume))
            {
                if (HasChildren)
                {
                    foreach (var child in childNodes)
                        child.Remove(leaf);
                }
                else
                    return leafs.Remove(leaf);
            }

            return false;
        }

        public void ProvideVolumes(ICollection<Volume<T>> result)
        {
            result.Add(NodeVolume);

            foreach (var child in childNodes)
                child.ProvideVolumes(result);
        }

        public void Find<TSearchType>(TSearchType searchType, List<ILeaf<T>> leafList, Func<TSearchType, Volume<T>, bool> overlap)
        {
            if(overlap(searchType, NodeVolume))
            {
                if (HasChildren)
                {
                    foreach (var child in childNodes)
                        child.Find(searchType, leafList, overlap);
                }
                else
                    leafList.AddRange(leafs);
            }
        }


        private void Split()
        {
            var childVolumes = tree.splitVolume(NodeVolume);
            foreach(var childVolume in childVolumes)
                childNodes.Add(tree.ProvideNode().Context(tree, childVolume, Level + 1));

            foreach (var leaf in leafs)
                AddToChildren(leaf);

            leafs.Clear();
        }

        private void AddToChildren(ILeaf<T> leaf)
        {
            foreach (var child in childNodes)
                child.Add(leaf);
        }
    }
}
