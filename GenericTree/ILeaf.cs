namespace GenericTree
{
    public interface ILeaf<T>
    {
        bool IntersectionCheck(Volume<T> volume);
    }
}
