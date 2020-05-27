using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree
{
    [DebuggerDisplay("Depth: {depth}; Leafs: {leafCount}; HasChildren: {childNodes.Count > 0}")]
    public class Node<T>
    {
        public Tree<T> Tree { get; private set; }
        public int Depth { get; private set; }
        public Volume<T> Volume { get { return volume; } }
        public int LeafCount { get; private set; }
        public IReadOnlyCollection<ILeaf<T>> Leafs { get { return leafs; } }
        public IReadOnlyCollection<Node<T>> ChildNodes { get { return childNodes; } }

        private readonly HashSet<ILeaf<T>> leafs;
        private readonly List<Node<T>> childNodes;
        private Volume<T> volume;

        public Node(Tree<T> tree)
        {
            Tree = tree;

            childNodes = new List<Node<T>>();
            leafs = new HashSet<ILeaf<T>>();
        }

        public virtual Node<T> Context(Volume<T> volume, int level)
        {
            Depth = level;
            this.volume = volume;

            childNodes.Clear();
            leafs.Clear();

            return this;
        }

        public virtual Node<T> Reset()
        {
            childNodes.Clear();
            leafs.Clear();
            LeafCount = 0;

            return this;
        }

        public virtual bool Add(ILeaf<T> leaf)
        {
            var success = false;
            
            if (leaf.IntersectionTest(Volume))
            {
                if (childNodes.Count > 0)
                {
                    success = AddToChildren(leaf);
                    if (success) LeafCount++;
                }
                else
                {
                    success = leafs.Add(leaf);
                    LeafCount = leafs.Count;

                    if (leafs.Count > Tree.maxLeafsPerNode
                        && Depth < Tree.maxDepth)
                        Split();
                }
            }

            return success;
        }

        public virtual bool Remove(ILeaf<T> leaf)
        {
            var success = false;
            
            if (leaf.IntersectionTest(Volume))
            {
                if (childNodes.Count > 0)
                {
                    foreach (var child in childNodes)
                        success |= child.Remove(leaf);
                    if (success) LeafCount--;
                }
                else
                {
                    success = leafs.Remove(leaf);
                    LeafCount = leafs.Count;
                }
            }

            return success;
        }

        public virtual void ListNodes(List<Node<T>> result, int minDepth = 0, int maxDepth = 0)
        {
            if (Depth > minDepth)
                result.Add(this);

            if (maxDepth > 0 && Depth < maxDepth)
                foreach (var child in childNodes)
                    child.ListNodes(result);
        }

        public virtual void ListVolumes(List<Volume<T>> result, int minDepth = 0, int maxDepth = 0)
        {
            if (Depth > minDepth)
                result.Add(Volume);

            if (maxDepth > 0 && Depth < maxDepth)
                foreach (var child in childNodes)
                    child.ListVolumes(result);
        }

        public virtual void Find<TSearchType>(TSearchType searchType, HashSet<ILeaf<T>> resultList, Func<TSearchType, Volume<T>, bool> intersectionCheck)
        {
            if (intersectionCheck(searchType, Volume))
            {
                if (childNodes.Count > 0)
                    foreach (var child in childNodes)
                        child.Find(searchType, resultList, intersectionCheck);
                else
                    foreach (var leaf in leafs)
                        resultList.Add(leaf);
            }
        }


        public virtual void TryMerge()
        {
            if (childNodes.Count > 0)
            {
                if (LeafCount <= Tree.maxLeafsPerNode)
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

        protected virtual void Disolve(HashSet<ILeaf<T>> loseLeafs)
        {
            if (childNodes.Count > 0)
                foreach (var child in childNodes)
                    child.Disolve(loseLeafs);
            else
                foreach (var leaf in leafs)
                    loseLeafs.Add(leaf);

            Tree.ReturnNode(this);
        }

        protected virtual void Split()
        {
            var childVolumes = Tree.volumeSplit(Volume);
            foreach (var childVolume in childVolumes)
                childNodes.Add(Tree.ProvideNode().Context(childVolume, Depth + 1));

            foreach (var leaf in leafs)
                AddToChildren(leaf);

            leafs.Clear();
        }

        protected virtual bool AddToChildren(ILeaf<T> leaf)
        {
            var success = false;

            foreach (var child in childNodes)
                success |= child.Add(leaf);

            return success;
        }
    }
}
