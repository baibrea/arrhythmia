using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject space1;
    [SerializeField] private GameObject space2;
    [SerializeField] private GameObject parent;
    public Grid gridObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridObject = new Grid(space1, space2, parent);
        gridObject.CreateFloorBox(-2, -2, 2, 2);
    }
}

[Serializable]
public class Grid
{
    /* 
    Key is a pair representing the xy coordinate of the grid space
    Value is a number representing the grid type:
        0 or if key doesn't exist = wall (not traversable by player)
        1 = floor (traversable by player)
        2 = ...
        3 = ...
    */
    private Dictionary<(int, int), int> grid;
    private GameObject space1;
    private GameObject space2;
    private GameObject parent;

    public Grid(GameObject space1, GameObject space2, GameObject parent)
    {
        this.space1 = space1;
        this.space2 = space2;
        this.parent = parent;
        this.grid = new Dictionary<(int, int), int>();
    }

    public int CheckSpace(int x, int y)
    {
        if (grid.ContainsKey((x, y)))
        {
            return grid[(x, y)];
        }
        else
        {
            return 0;
        }
    }

    public void SetSpace(int x, int y, int type)
    {
        grid[(x, y)] = type;
    }

    public void CreateFloorBox(int x1, int y1, int x2, int y2) 
    {
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                grid[(i, j)] = 1;
                if ((i + j) % 2 == 0)
                {
                    GameObject.Instantiate(space1, new Vector3(i, 0, j), Quaternion.identity, parent.transform);
                } else
                {
                    GameObject.Instantiate(space2, new Vector3(i, 0, j), Quaternion.identity, parent.transform);
                }
            }
        }
    }

}
