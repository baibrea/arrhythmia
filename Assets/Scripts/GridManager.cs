using System;
using System.Collections.Generic;
using UnityEngine;
using DigitalWorlds.StarterPackage3D;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private GameObject space1;
    [SerializeField] private GameObject space2;
    [SerializeField] private GameObject parent;
    public Grid gridObject;

    [Header("Door Settings")]
    public Dictionary<(int, int), Lock3D> doors = new Dictionary<(int, int), Lock3D>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gridObject = new Grid(space1, space2, parent);
        gridObject.CreateFloorBox(-2, -2, 2, 2);
    }

    // RegisterDoor() initalizes a space in the grid as a door type, then adds it to the doors dictionary
    public void RegisterDoor(int x, int y, Lock3D door)
    {
        gridObject.SetSpace(x, y, "door");
        doors[(x, y)] = door;
    }

    // CheckDoor() returns the Lock3D object at the x and y coordinates if it exists
    // otherwise returns null
    public Lock3D CheckDoor(int x, int y)
    {
        if (doors.ContainsKey((x, y)))
        {
            return doors[(x, y)];
        }
        else
        {
            return null;
        }
    }
}

[Serializable]
public class Grid
{
    /* 
    Key is a pair representing the xy coordinate of the grid space
    Value is a string representing the grid type:
        wall (not traversable by player)
        (traversable by player)
        door...
        etc...
    */
    private Dictionary<(int, int), String> grid;
    private GameObject space1;
    private GameObject space2;
    private GameObject parent;

    public Grid(GameObject space1, GameObject space2, GameObject parent)
    {
        this.space1 = space1;
        this.space2 = space2;
        this.parent = parent;
        this.grid = new Dictionary<(int, int), String>();
    }

    public String CheckSpace(int x, int y)
    {
        if (grid.ContainsKey((x, y)))
        {
            return grid[(x, y)];
        }
        else {
            return "wall";
        }
    }

    public void SetSpace(int x, int y, String type)
    {
        grid[(x, y)] = type;
    }

    public void CreateFloorBox(int x1, int y1, int x2, int y2)
    {
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                grid[(i, j)] = "floor";
                if ((i + j) % 2 == 0)
                {
                    GameObject.Instantiate(space1, new Vector3(i, 0, j), Quaternion.identity, parent.transform);
                }
                else
                {
                    GameObject.Instantiate(space2, new Vector3(i, 0, j), Quaternion.identity, parent.transform);
                }
            }
        }
    }
}
