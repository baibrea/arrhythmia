using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Grid gridObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridObject = new Grid();
        gridObject.CreateFloorBox(-2, -2, 2, 2);
    }
}

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
    private Dictionary<(int, int), int> grid = new Dictionary<(int, int), int>();

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
        int length = Mathf.Abs(x2 - x1);
        int width = Mathf.Abs(y2 - y1);

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[(i, j)] = 1;
            }
        }
    }

}
