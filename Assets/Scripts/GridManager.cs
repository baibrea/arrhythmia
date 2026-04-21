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
    [SerializeField] private bool generatePrefabs = false;

     /* 
    Key is a pair representing the xy coordinate of the grid space
    Value is a string representing the grid type:
        wall (not traversable by player)
        (traversable by player)
        door...
        etc...
    */
    private Dictionary<(int, int), String> grid = new Dictionary<(int, int), String>();


    [Header("Door Settings")]
    public Dictionary<(int, int), Lock3D> doors = new Dictionary<(int, int), Lock3D>();

    // RegisterDoor() initalizes a space in the grid as a door type, then adds it to the doors dictionary
    public void RegisterDoor(int x, int y, Lock3D door)
    {
        SetTile(x, y, "door");
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

    public void SetTile(int x, int y, String type)
    {
        grid[(x, y)] = type;
    }

    public String GetTile(int x, int y) 
    {
        if (grid.ContainsKey((x, y)))
        {
            return grid[(x, y)];
        }
        else
        {
            return "wall";
        }
    }

    public void FillFloor(int x1, int y1, int x2, int y2)
    {
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                SetTile(i, j, "floor");

                if (generatePrefabs)
                {
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

    public void FillWall(int x1, int y1, int x2, int y2)
    {
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                SetTile(i, j, "wall");
            }
        }
    }

}