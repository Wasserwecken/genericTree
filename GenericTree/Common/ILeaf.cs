namespace GenericTree.Common
{
    public interface ILeaf<T>
    {
        bool IntersectionTest(Volume<T> volume);
    }
}
