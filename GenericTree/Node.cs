using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree
{
    [DebuggerDisplay("Depth: {depth}; Leafs: {leafCount}; HasChildren: {childNodes.Count > 0}")]
    internal class Node<T>
    {
        private int depth;
        private Volume<T> volume;
        private Tree<T> tree;
        private readonly HashSet<ILeaf<T>> leafs;
        private readonly List<Node<T>> childNodes;
        private int leafCount;


        public Node()
        {
            childNodes = new List<Node<T>>();
            leafs = new HashSet<ILeaf<T>>();
        }

        public Node<T> Context(
            Tree<T> tree,
            Volume<T> volume,
            int level)
        {
            this.tree = tree;
            this.volume = volume;
            this.depth = level;

            childNodes.Clear();
            leafs.Clear();

            return this;
        }

        public Node<T> Reset()
        {
            tree = null;
            childNodes.Clear();
            leafs.Clear();
            leafCount = 0;

            return this;
        }

        public bool Add(ILeaf<T> leaf)
        {
            var success = false;
            
            if (leaf.CheckOverlap(volume))
            {
                if (childNodes.Count > 0)
                {
                    success = AddToChildren(leaf);
                    if (success) leafCount++;
                }
                else
                {
                    success = leafs.Add(leaf);
                    leafCount = leafs.Count;

                    if (leafs.Count > tree.settings.maxNodeLeafs
                        && depth < tree.settings.maxDepth)
                        Split();
                }
            }

            return success;
        }

        public bool Remove(ILeaf<T> leaf)
        {
            var success = false;
            
            if (leaf.CheckOverlap(volume))
            {
                if (childNodes.Count > 0)
                {
                    foreach (var child in childNodes)
                        success |= child.Remove(leaf);
                    if (success) leafCount--;
                }
                else
                {
                    success = leafs.Remove(leaf);
                    leafCount = leafs.Count;
                }
            }

            return success;
        }

        public void ProvideVolumes(List<Volume<T>> result)
        {
            result.Add(volume);

            foreach (var child in childNodes)
                child.ProvideVolumes(result);
        }

        public void Find<TSearchType>(TSearchType searchType, HashSet<ILeaf<T>> resultList, Func<TSearchType, Volume<T>, bool> overlap)
        {
            if (overlap(searchType, volume))
            {
                if (childNodes.Count > 0)
                    foreach (var child in childNodes)
                        child.Find(searchType, resultList, overlap);
                else
                    foreach (var leaf in leafs)
                        resultList.Add(leaf);
            }
        }


        public void TryMerge()
        {
            if (childNodes.Count > 0)
            {
                if (leafCount <= tree.settings.maxNodeLeafs)
                {
                    foreach (var child in childNodes)
                        child.Disolve(leafs);
                    childNodes.Clear();
                }
                else
                    foreach (var child in childNodes)
                        child.TryMerge();
            }
        }

        private void Disolve(HashSet<ILeaf<T>> loseLeafs)
        {
            if (childNodes.Count > 0)
                foreach (var child in childNodes)
                    child.Disolve(loseLeafs);
            else
                foreach (var leaf in leafs)
                    loseLeafs.Add(leaf);

            tree.ReturnNode(this);
        }

        private void Split()
        {
            var childVolumes = tree.splitVolume(volume);
            foreach (var childVolume in childVolumes)
                childNodes.Add(tree.ProvideNode().Context(tree, childVolume, depth + 1));

            foreach (var leaf in leafs)
                AddToChildren(leaf);

            leafs.Clear();
        }

        private bool AddToChildren(ILeaf<T> leaf)
        {
            var success = false;

            foreach (var child in childNodes)
                success |= child.Add(leaf);

            return success;
        }
    }
}
