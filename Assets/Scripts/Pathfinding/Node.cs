using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool _walkable;
    public Vector3 _worldPosition;
    public int _gridX;
    public int _gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        _walkable = walkable;
        _worldPosition = worldPosition;
        _gridX = gridX;
        _gridY = gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
