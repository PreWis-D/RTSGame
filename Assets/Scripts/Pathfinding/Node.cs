using UnityEngine;

public class Node
{
    public bool _walkable;
    public Vector3 _worldPosition;
    public int _gridX;
    public int _gridY;

    public int gCost;
    public int hCost;
    public Node parent;

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
}
