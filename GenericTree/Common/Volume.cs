using System.Diagnostics;

namespace GenericTree.Common
{
    [DebuggerDisplay("Origin: {origin}; Size: {size}")]
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
