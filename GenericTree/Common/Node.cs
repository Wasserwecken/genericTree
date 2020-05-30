using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("depth: {depth}, Leafs: {LeafCount}, Children: {ChildNodes.Count > 0}, Volume: {Volume}")]
    public class Node<T>
    {
        protected Volume<T> volume;
        protected int leafCount;
        protected int depth;

        protected int maxDepth;
        protected int maxLeafsPerNode;
        protected Func<Volume<T>, Volume<T>[]> volumeSplit;

        protected readonly HashSet<ILeaf<T>> leafs;
        protected readonly List<Node<T>> childNodes;
        
        protected static readonly Stack<Node<T>> unusedNodes;


        public Node()
        {
            childNodes = new List<Node<T>>();
            leafs = new HashSet<ILeaf<T>>();
        }


        protected virtual Node<T> ProvideNode()
            => unusedNodes.Count > 0 ? unusedNodes.Pop() : new Node<T>();

        protected virtual Node<T> SetContext(
            Volume<T> volume,
            int depth,
            int maxDepth,
            int maxLeafsPerNode,
            Func<Volume<T>, Volume<T>[]> volumeSplit)
        {
            this.volume = volume;
            this.depth = depth;
            this.maxDepth = maxDepth;
            this.maxLeafsPerNode = maxLeafsPerNode;
            this.volumeSplit = volumeSplit;

            leafCount = 0;
            childNodes.Clear();
            leafs.Clear();

            return this;
        }


        public virtual bool Add(ILeaf<T> leaf)
        {
            var success = false;
            
            if (leaf.IntersectionTest(volume))
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

                    if (leafs.Count > maxLeafsPerNode && depth < maxDepth)
                        Split();
                }
            }

            return success;
        }

        public virtual bool Remove(ILeaf<T> leaf)
        {
            var success = false;
            
            if (leaf.IntersectionTest(volume))
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

        public virtual void Find<TSearchType>(TSearchType searchType, HashSet<ILeaf<T>> resultList, Func<TSearchType, Volume<T>, bool> intersectionTest)
        {
            if (intersectionTest(searchType, volume))
            {
                if (childNodes.Count > 0)
                    foreach (var child in childNodes)
                        child.Find(searchType, resultList, intersectionTest);
                else
                    foreach (var leaf in leafs)
                        resultList.Add(leaf);
            }
        }


        protected virtual void TryMerge()
        {
            if (childNodes.Count > 0)
            {
                if (leafCount <= maxLeafsPerNode)
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
            foreach (var child in childNodes)
                child.Disolve(loseLeafs);
            childNodes.Clear();

            foreach (var leaf in leafs)
                loseLeafs.Add(leaf);
            leafs.Clear();

            unusedNodes.Push(this);
        }

        protected virtual void Split()
        {
            var childVolumes = volumeSplit(volume);
            foreach (var childVolume in childVolumes)
                childNodes.Add(ProvideNode().SetContext(childVolume, depth + 1, maxDepth, maxLeafsPerNode, volumeSplit));

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
