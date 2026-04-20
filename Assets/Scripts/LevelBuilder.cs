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

        // First Level

        // grid.FillFloor(-25, 11, -18, 17);
        // grid.FillFloor(-28, 15, -23, 20);

        // Rooms for first level
        grid.FillFloor(-28, 11, -18, 12);
        grid.FillFloor(-34, 13, -30, 18);
        grid.FillFloor(-28, 13, -27, 27);
        grid.FillFloor(-25, 18, -21, 23);
        grid.FillFloor(-27, 26, -18, 27);
        grid.FillFloor(-19, 13, -18, 25);

        // Doors for first level
        grid.SetTile(-29, 14, "door");
        grid.SetTile(-29, 17, "door");
        grid.SetTile(-26, 19, "door");
        grid.SetTile(-26, 22, "door");

        // Walls


        // Second Level
        grid.FillFloor(-16, 11, -11, 20);

        // Third Level
        grid.FillFloor(-9, 11, -2, 18);
    }

}
