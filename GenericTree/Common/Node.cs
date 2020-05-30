using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("Depth: {Depth}; Leafs: {LeafCount}; Children: {ChildNodes.Count > 0}; Volume: {Volume}")]
    public class Node<T>
    {
        public Tree<T> Tree { get; private set; }
        public int Depth { get; private set; }
        public Volume<T> Volume { get { return volume; } }
        public int LeafCount { get; private set; }

        protected readonly HashSet<ILeaf<T>> leafs;
        protected readonly List<Node<T>> childNodes;
        protected Volume<T> volume;
        
        protected static readonly Stack<Node<T>> unusedNodes;


        public Node(Tree<T> tree)
        {
            Tree = tree;
            LeafCount = 0;
            Depth = 0;

            childNodes = new List<Node<T>>();
            leafs = new HashSet<ILeaf<T>>();
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

            unusedNodes.Push(Reset());
        }

        protected virtual void Split()
        {
            var childVolumes = Tree.VolumeSplit(Volume);
            foreach (var childVolume in childVolumes)
                childNodes.Add(ProvideNode().SetContext(childVolume, Depth + 1));

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


        protected virtual Node<T> ProvideNode()
            => unusedNodes.Count > 0 ? unusedNodes.Pop() : new Node<T>(Tree);

        protected virtual Node<T> SetContext(Volume<T> volume, int level)
        {
            Depth = level;
            this.volume = volume;

            LeafCount = 0;
            childNodes.Clear();
            leafs.Clear();

            return this;
        }

        protected virtual Node<T> Reset()
        {
            LeafCount = 0;
            childNodes.Clear();
            leafs.Clear();

            return this;
        }
    }
}
