# genericTree
This projekt is a private case study about abstracting the quad and octree process, to enable a generic leaf bounding boxes and a dimensional and position type lose implementation.


## Features
- Presets for a standard octree and quadtree
- Supports generic types for position and size
- Supports any bounding box for leafs
- Can be extended into any dimension


### Not included
- No presets for ray or complex intersections
- No presets for a higher dimensional tree


## Preset usage
The example uses the octree preset. Nevertheless the API and behaviour is exactly the same for the quadtree.   


### Initialisation
```c#
// defines the area of the tree
var startVolume = new Volume<Vector3>(
        Vector3.Zero,
        Vector3.One * 10f
    );

// creates a tree instance for working with leafs
var tree = new Octree(startVolume, 10, 4);
```
The instance of the tree is used to interact with the structure and the containing leafs. The leaf itself will have no influence or knowledge about the structure.


### Leafs
To add an item to the tree structure, it needs to be marked as a leaf by implementing the interface `ILeaf<T>` where `T` is the type of the position data. In case of the octree preset `T` has to be `Vector3`.

```c#
class Item : ILeaf<Vector3>
{
    // position in 3D space of the item 
    private Vector3 position;

    // method from the interface
    public bool CheckOverlap(Volume<Vector3> volume)
    {
        // intersection test from the octree preset can be used
        return Octree.OverlapTest.PointOverlap(position, volume);
    }
}
```

The method `bool CheckOverlap(Volume<Vector3> volume)` is called by the tree when the structure is change (usually this will happen on Add or Remove). With this method, the tree will determine how to build and where an element is located in the structure.

Due to outsourcing the intersection test to the leaf, the user can dicide which bouding shape a leaf implements. The example above used a point as bouding shape. Following example will implement a item with a box as bounding shape:

```c#
class BoxItem : ILeaf<Vector3>
{
    // position and size in the 3D space of the item 
    private Vector3 position;
    private Vector3 size;

    // method from the interface
    public bool CheckOverlap(Volume<Vector3> volume)
    {
        // intersection test from the octree preset can be used
        var box = new Octree.OverlapType.Box(position, size);
        return Octree.OverlapTest.BoxOverlap(box, volume);
    }
}
```

If there is a need for a custom bounding shape, only the intersection code has to be written for it. `Volume<T>` is always a AABB Box.


### Tree interaction

#### Add
```c#
// prepares the tree
var startVolume = new Volume<Vector3>(Vector3.Zero, Vector3.One * 10f );
var tree = new Octree(startVolume, 10, 4);

// prepares the items
var pointItem = new PointItem();
var boxItem = new BoxItem();

// adds them to the structure
tree.Add(pointItem);
tree.Add(boxItem);
```

For adding leafs, the add method on the tree instance have to be called. A leafs can be in more than in one tree instance. Also all kind of leafs can be added to a tree, there is no constraint, due to the `ILeaf<T>` interface. By adding a new leaf the tree will handle its structural organisation by itself. Adding a leaf again, will return `false`.


#### Remove

```c#
tree.Remove(pointItem);
```

By removing a leaf, the tree will try to collaps the internal build structure for performance.
> IMPORTANT! This method is also using the bounding shape of the leaf to locate it in the internal structure. If the position has be changed, the leaf may cannot be removed.


#### Update

```c#
tree.Remove(pointItem);
pointItem.Move();
tree.Add(boxItem);
```

There its no Update() method for the tree structure. To update the position of a leaf, it simply have to be removed, then moved and finnally added again. If the items is moved before the remove, the item may cannot be found.


### Search
The intend of a tree structure is to accelerate the search of items for a given area.
This can be done with:

```c#            
var result1 = tree.SearchByPoint(Vector3.Zero);
var result2 = tree.SearchByBox(Vector3.Zero, Vector3.Zero);
var result3 = tree.SearchBySphere(Vector3.Zero, 1f);
```

The search returns a `HashSet<ILeaf<T>>` of all leafs that may have an intersection with the given shape. The search only check for a intersection with the volume and sub-volumes of the tree structure. A final intersection test on the leafs itself have to be done by the user, if needed.

If a search has do be done by a another shape (e.g. a Ray, Capsule, OBB Box), the generic search method have to be used.

```c#
// search for leafs near by a ray           
var result = tree.Search(new Ray, RayBoxIntersection);

// intersection method which is used by the search
// has to be written by the user
public bool RayBoxIntersection(Ray ray, Volume<Vector3> volume)
{
    // interserction code
    // ...

    return true;
}
```

## Custom tree
For custom types other than `System.Numerics.Vector2`, `System.Numerics.Vector3` or a higher dimensional vector, the generic `Tree<T>` class have to be used.

```c#
public void TreeInit()
{
    var startVolume = new Volume<Vector4>(
            Vector4.Zero,
            Vector4.One * 10f
        );
    var tree4D = new Tree<Vector4>(startVolume, 10, 4, SplitVolume);
}

// this method is called by the tree if the internal structure have to grow
public Volume<Vector4>[] SplitVolume(Volume<Vector4> volume)
{
    // create new subvolumes based of the given volume
    // ...

    return newVolumes
}
```

THe initialisation of a custom tree is quite the same as using a preset. The generic tree only needs a additional method for splitting volumes. The method is called when a node of the tree contains more leafs than allowed and the maximum depth is not reached. All other functions are used like with the presets, except the search.

For the searching in the generic tree, only the generic search is available (see: Search). I recommend to create a extra class for custom trees which extends the `Tree<T>` (see: octree preset implelemtation source).