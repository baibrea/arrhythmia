using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildTutorial();
        BuildLevels();
    }

    void BuildTutorial()
    {
        grid.FillFloor(-4, -3, 4, 3);
        grid.SetTile(-5, 0, "door");
        grid.SetTile(-2, -2, "wall");
        grid.SetTile(-1, -2, "wall");
        grid.SetTile(3, -3, "wall");
        grid.SetTile(4, 2, "wall");
        grid.SetTile(3, 3, "wall");
        grid.SetTile(4, 3, "wall");
        grid.SetTile(0, 2, "wall");
        grid.SetTile(-2, 2, "wall");
        grid.SetTile(3, -2, "wall");
    }

    void BuildLevels()
    {
        // Main Floor
        grid.FillFloor(-21, -3, -6, 3);

        // Hallways
        grid.FillFloor(-19, 4, -18, 10);
        grid.FillFloor(-14, 4, -13, 10);
        grid.FillFloor(-9, 4, -8, 10);

        // Final Hallway Doors
        grid.SetTile(-14, 4, "door");
        grid.SetTile(-13, 4, "door");

        // Hospital Level

        // grid.FillFloor(-25, 11, -18, 17);
        // grid.FillFloor(-28, 15, -23, 20);

        // Rooms for hospital level
        grid.FillFloor(-28, 11, -18, 12);
        grid.FillFloor(-36, 13, -30, 18);
        grid.FillFloor(-28, 13, -27, 27);
        grid.FillFloor(-25, 18, -21, 22);
        grid.FillFloor(-27, 26, -18, 27);
        grid.FillFloor(-19, 14, -18, 25);
        grid.FillFloor(-26, 29, -20, 35);
        grid.FillFloor(-29, 14, -29, 14);
        grid.FillFloor(-29, 17, -29, 17);
        grid.FillFloor(-26, 19, -26, 19);
        grid.FillFloor(-26, 22, -26, 22);
        grid.FillFloor(-23, 28, -23, 28);

        // Doors for hospital level
        // grid.SetTile(-29, 14, "door");
        // grid.SetTile(-29, 17, "door");
        // grid.SetTile(-26, 19, "door");
        // grid.SetTile(-25, 22, "door");
        // grid.SetTile(-23, 28, "door");

        // Walls for hospital level
        grid.SetTile(-32, 16, "wall");
        grid.SetTile(-33, 16, "wall");
        grid.SetTile(-32, 16, "wall");
        grid.SetTile(-33, 15, "wall");
        grid.SetTile(-36, 16, "wall");
        grid.SetTile(-35, 16, "wall");
        grid.SetTile(-36, 18, "wall");
        grid.SetTile(-34, 13, "wall");
        grid.SetTile(-31, 16, "wall");

        // Additional walls for hospital level
        grid.SetTile(-22, 19, "wall");
        grid.SetTile(-21, 19, "wall");
        grid.SetTile(-21, 18, "wall");
        grid.SetTile(-21, 20, "wall");
        grid.SetTile(-22, 22, "wall");
        grid.SetTile(-21, 22, "wall");
        grid.SetTile(-21, 23, "wall");

        grid.SetTile(-25, 30, "wall");
        grid.SetTile(-25, 31, "wall");
        grid.SetTile(-26, 33, "wall");
        grid.SetTile(-25, 34, "wall");
        grid.SetTile(-26, 34, "wall");
        grid.SetTile(-26, 35, "wall");
        grid.SetTile(-20, 29, "wall");
        grid.SetTile(-21, 35, "wall");
        grid.SetTile(-20, 34, "wall");
        grid.SetTile(-20, 35, "wall");

        grid.SetTile(-28, 20, "wall");
        grid.SetTile(-28, 21, "wall");
        grid.SetTile(-28, 22, "wall");

        // Second Level
        grid.FillFloor(-16, 11, -11, 20);

        // Third Level
        grid.FillFloor(-9, 11, -2, 18);
    }

    public void BuildPath()
    {
        grid.FillFloor(-19, 13, -18, 13);
    }

}
