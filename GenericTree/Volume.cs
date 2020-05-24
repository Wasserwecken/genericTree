namespace GenericTree
{
    public struct Volume<T>
    {
        public T center;
        public T size;

        public Volume(T center, T size)
        {
            this.center = center;
            this.size = size;
        }
    }
}
