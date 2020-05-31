using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("Depth: {Depth}, Leafs: {LeafCount}, Volume: {Volume}")]
    public abstract class RootBase<T, TNode> where TNode : NodeBase<T, TNode>, new()
    {
        public int MaxDepth { get; private set; }
        public int MaxLeafsPerNode { get; private set; }
        
        public Volume<T> Volume => volume; 
        public int LeafCount => rootNode.LeafCount;
        public int Depth => rootNode.CheckMaxDepth();

        protected Volume<T> volume;
        protected TNode rootNode;


        public RootBase(T origin, T size, int maxDepth, int maxLeafsPerNode)
        {
            MaxDepth = maxDepth;
            MaxLeafsPerNode = maxLeafsPerNode;
            volume = new Volume<T>(origin, size);

            rootNode = CreateRootNode();
        }

        public virtual bool Add(ILeaf<T> leaf)
            => rootNode.Add(leaf);

        public bool Remove(ILeaf<T> leaf)
        {
            var result = rootNode.Remove(leaf);
            if (result) rootNode.TryMerge();

            return result;
        }

        public virtual void FindBy<TSearchType>(HashSet<ILeaf<T>> result, TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersectionTest)
            => rootNode.FindBy(searchType, result, intersectionTest);

        public virtual HashSet<ILeaf<T>> FindBy<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersectionTest)
        {
            var result = new HashSet<ILeaf<T>>();
            rootNode.FindBy(searchType, result, intersectionTest);

            return result;
        }

        public virtual void ListLeafs(HashSet<ILeaf<T>> result)
            => rootNode.ListLeafs(result);

        public virtual HashSet<ILeaf<T>> ListLeafs()
        {
            var result = new HashSet<ILeaf<T>>();
            rootNode.ListLeafs(result);

            return result;
        }

        public virtual void ListVolumes(List<Volume<T>> result)
            => rootNode.ListVolumes(result);

        public virtual List<Volume<T>> ListVolumes()
        {
            var result = new List<Volume<T>>();
            rootNode.ListVolumes(result);

            return result;
        }

        protected abstract TNode CreateRootNode();
    }
}
