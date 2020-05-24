namespace GenericTree
{
    public struct Volume<T>
    {
        public T origin;
        public T size;

        public Volume(T origin, T size)
        {
            this.origin = origin;
            this.size = size;
        }
    }
}
