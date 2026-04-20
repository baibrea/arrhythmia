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

        // First Level
        grid.FillFloor(-25, 11, -18, 17);
        grid.FillFloor(-28, 15, -23, 20);

        // Second Level
        grid.FillFloor(-16, 11, -11, 20);

        // Screens Room
        grid.FillFloor(-9, 11, -2, 18); // Entry Room
        grid.FillFloor(-1, 17, 0, 18); // Walkway to 2nd room
        grid.FillFloor(1, 11, 10, 22); // 2nd room
        grid.FillFloor(-1, 6, 10, 9); // 3rd bottom area
        grid.FillFloor(9, 10, 10, 10); // Connecting line to 3rd bottom area

        // Screens Room Exit Path
        // TODO: make it appear after the player collects the key
        grid.FillFloor(-7, 7, -1, 8);
    }

}
