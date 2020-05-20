namespace GenericTree
{
    public interface ILeaf<T>
    {
        bool CheckOverlap(Volume<T> volume);
    }
}
