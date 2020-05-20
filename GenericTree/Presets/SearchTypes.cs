using System.Numerics;

namespace GenericTree.Presets
{
    public struct Box
    {
        public Vector3 center;
        public Vector3 size;

        public Box(Vector3 center, Vector3 size)
        {
            this.center = center;
            this.size = size;
        }
    }

    public struct Sphere
    {
        public Vector3 center;
        public float radius;

        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }
}
