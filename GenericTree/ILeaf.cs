namespace GenericTree
{
    public interface ILeaf<T>
    {
        bool IntersectionTest(Volume<T> volume);
    }
}
