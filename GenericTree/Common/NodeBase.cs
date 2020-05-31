using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("Level: {Level}, Leafs: {LeafCount}, Children: {childNodes.Count > 0}")]
    public abstract class NodeBase<T, TNode> where TNode : NodeBase<T, TNode>, new()
    {
        public Volume<T> Volume { get; private set; }
        public int LeafCount { get; private set; }
        public int Level { get; private set; }

        protected int maxDepth;
        protected int maxLeafsPerNode;
        protected Func<Volume<T>, Volume<T>[]> volumeSplit;

        protected readonly HashSet<ILeaf<T>> leafs;
        protected readonly List<TNode> childNodes;

        protected static readonly Stack<TNode> unusedNodes = new Stack<TNode>();


        public NodeBase()
        {
            childNodes = new List<TNode>();
            leafs = new HashSet<ILeaf<T>>();
        }

        public NodeBase(
            Volume<T> volume,
            int depth,
            int maxDepth,
            int maxLeafsPerNode,
            Func<Volume<T>, Volume<T>[]> volumeSplit)
            : this()
        {
            SetContext(volume, depth, maxDepth, maxLeafsPerNode, volumeSplit);
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

                    if (leafs.Count > maxLeafsPerNode && Level < maxDepth)
                        Extend();
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

        public virtual void FindBy<TSearchType>(TSearchType searchType, HashSet<ILeaf<T>> resultList, Func<TSearchType, Volume<T>, bool> intersectionTest)
        {
            if (intersectionTest(searchType, Volume))
                if (childNodes.Count > 0)
                    foreach (var child in childNodes)
                        child.FindBy(searchType, resultList, intersectionTest);
                else
                    foreach (var leaf in leafs)
                        resultList.Add(leaf);
        }

        public virtual int CheckMaxDepth()
        {
            var result = Level;

            foreach (var child in childNodes)
                result = Math.Max(result, child.CheckMaxDepth());

            return result;
        }

        public virtual void ListLeafs(HashSet<ILeaf<T>> result)
        {
            foreach (var leaf in leafs)
                result.Add(leaf);

            foreach (var child in childNodes)
                child.ListLeafs(result);
        }

        public virtual void ListVolumes(List<Volume<T>> result)
        {
            result.Add(Volume);

            foreach (var child in childNodes)
                child.ListVolumes(result);
        }


        protected virtual TNode ProvideNode()
            => unusedNodes.Count > 0 ? unusedNodes.Pop() : new TNode();

        protected virtual TNode SetContext(
            Volume<T> volume,
            int level,
            int maxDepth,
            int maxLeafsPerNode,
            Func<Volume<T>, Volume<T>[]> volumeSplit)
        {
            if (maxDepth < 0)
                throw new ArgumentException("Negative depth is invalid");

            if (maxLeafsPerNode <= 0)
                throw new ArgumentException("At least one leaf per node have to be allowed");

            this.Volume = volume;
            this.Level = level;
            this.maxDepth = maxDepth;
            this.maxLeafsPerNode = maxLeafsPerNode;
            this.volumeSplit = volumeSplit;

            LeafCount = 0;
            childNodes.Clear();
            leafs.Clear();

            return (TNode)this;
        }


        public virtual void TryMerge()
        {
            if (childNodes.Count > 0)
            {
                if (LeafCount <= maxLeafsPerNode)
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

            unusedNodes.Push((TNode)this);
        }

        protected virtual void Extend()
        {
            var childVolumes = volumeSplit(Volume);
            foreach (var childVolume in childVolumes)
                childNodes.Add(ProvideNode().SetContext(childVolume, Level + 1, maxDepth, maxLeafsPerNode, volumeSplit));

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
