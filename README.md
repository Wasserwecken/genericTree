# genericTree
Case study about seperating quad and octree logic for generic bounding boxes, position types and dimensions.


## Features
- Presets - Octree, Quadtree, TreeTree
- Supports generic position types (e.g. Vector4)
- Supports any bounding box
- Works in any dimension


### Not implemented
- Hardware acceleration for FreeTree.
- Dynamic leafs (moving, resizing)
- Intersection tests for rays or complex volumes (e.g Mesh, Capsules, OBB).


## Presets
Following presets are available:
- Quadtree *(uses System.Vector2)*
- Octree *(uses System.Vector3)*
- FreeTree *(uses Generic.Vector)*

The API of the presets behaves and works identical. Only the positional type differs. The octree preset is used for all examples, but they can be applied on the other presets;


## Initialisation

```c#
// settings
var origin = Vector3.Zero;
var size = Vector3.One * 5f;
var maxDepth = 5;
var maxLeafsPerNode = 5;

// initialisation
var tree = new Octree(origin, size, maxDepth, maxLeafsPerNode);
```

`origin` and `size` defining the area where the octree will manage. Only items withing this range used.  
`maxDepth` and `maxLeafsPerNode`are used to control the division of the area of the tree. They will also have an impact on the performance. 


## Leafs
Leafs are items which can be added to a tree. By implementing the interface `ILeaf<T>`, any class
can be added to any tree.

> **Important Note**  
The tree expects static leafs. If a leaf changes it position, it may can not be found or removed anymore, because these functions using the position / boundings for the search. If a leaf needs to be moved or resized, it has to be removed from the tree first, then modified, an finally added again back to the tree.

```c#
using System.Numerics;

class Item : ILeaf<Vector3>
{
    // Box is part of the "GenericTree.Octree" namepace
    // defines the position and size of the item
    public Box boundingBox;

    // method from the interface
    public bool IntersectionTest(Volume<Vector3> volume)
    {
        // tests if the item overlaps with the volume or is inside it
        // by using the intersection test from the Box type
        return boundingBox.TestIntersection(volume);
    }
}
```

The type of `ILeaf<T>` depends which tree is used. The preset octree is using `System.Vector3` for processing, the leaf has to use the same type.  

The interface requires the implementation of the method `bool IntersectionCheck(Volume<Vector3> volume)`. Which is call from the tree, to decide how to build the tree structure on adding and finding leafs for remove

To test for a intersection, following types providing the method `bool TestIntersection(Volume<Vector2> volume)`
- Point
- Box
- Sphere *(Circle for Quadtree)*

If a leaf has a bounding shape other the already implemented one, the intersection test have to be implemented from the user.  

This structure allows to add any leaf with any shape to be part of a tree. It is also possible to add a leaf to multiple trees.


## Add & remove

```c#
public class Example
{
    public void Foo()
    {
        // init
        var tree = new Octree(Vector3.Zero, Vector3.One, 5, 5);
        var item = new Item();

        // add & remove
        tree.Add(item);
        tree.Remove(item);
    }
}

public class Item : ILeaf<Vector3>
{
    public Sphere boundingSphere;

    public bool IntersectionTest(Volume<Vector3> volume)
        => boundingSphere.TestIntersection(volume);
}
```

Leafs can be either removed od added by using the methods on the tree. They both return a boolean as result for the success.  
They will return false if the leaf
- has no overlap with tree area
- has been already added / removed from the tree
- cannot be found (*for remove only*)


## Search

```c#
public class Example
{
    public void Foo()
    {
        // init
        var tree = new Octree(Vector3.Zero, Vector3.One * 10, 5, 5);
        for(int i = 0; i < 100000; i++)
            tree.Add(new Item());

        // search
        var origin = new Vector3(1f, 2f, 4f);
        var size = new Vector3(4f, 1f, 0.5f);
        var result = tree.FindByBox(origin, size);
    }
}
```
The search will return a `HasSet<ILeaf<T>>` will all relevant leafs for the given search area. Like the intersection tests, the presets will offer search methods for searching with a Box, Sphere or Point.

> **Important Note**  
The search will not test for intersection with the leaf and the search area! The search will return all leafs that are in nodes where the volume of the node has an overlap with the search area. A final intersection test has to be done by the user.


### Custom search shape

```c#
public class Example
{
    public void Foo()
    {
        // init
        var tree = new Octree(Vector3.Zero, Vector3.One * 10, 5, 5);
        for(int i = 0; i < 100000; i++)
            tree.Add(new Item());

        // search by a ray
        var result = tree.FindBy(new Ray(), RayBoxIntersection);
    }

    // custom intersection function for the ray search
    private static bool RayBoxIntersection(Ray ray, Volume<Vector3> volume)
    {
        // intersection logic
        // ...

        return result;
    }
}

// custom search type
public struct Ray
{
    public Vector3 origin;
    public Vector3 direction;
    public float length;

    // ...
}
```

If the search area / shape needs to have a diffrent shape than a box, sphere or point, the generic method `HashSet<ILeaf<T>> FindBy<TSearchType>(TSearchType searchType, Func<TSearchType, Volume<T>, bool> intersectionTest)` has to be used. The method requires a
- search type
- intersection test for the search type and `Volume<T>`

If both is given, the tree can be search by any shape. `Volume<T>` is used by the tree for defining its area and node volumes. It behaves as a AABB Box.


## Custom tree
To use other position types other than `System.Numerics.Vector2`, `System.Numerics.Vector3` or `GenericVector.Vector`, the class `RootNode<T>` have to be extended.
