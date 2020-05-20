namespace GenericTree
{
    public struct Volume<T>
    {
        public T center;
        public float size;

        public Volume(T center, float size)
        {
            this.center = center;
            this.size = size;
        }
    }
}
