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
        grid.FillWall(-7, 11, -7, 13); // Wall along entry hallway
        grid.FillWall(-5, 13, -3, 13); // Bottom wall
        grid.FillWall(-4, 16, -4, 18); // Top wall
        grid.FillFloor(-1, 17, 0, 18); // Walkway to 2nd room

        grid.FillFloor(1, 12, 10, 22); // 2nd room floor
        grid.FillWall(3, 19, 4, 19); // 2nd room, top left wall
        grid.FillWall(7, 18, 7, 20); // 2nd room, top right wall
        grid.SetTile(8, 18, "wall");

        grid.FillWall(1, 15, 3, 16); // 2nd room, left wall
        grid.FillWall(6, 13, 10, 14); // 2nd room, bottom right wall

        grid.FillFloor(9, 8, 10, 11); // Walkway to final hallway
        grid.FillFloor(-1, 7, 10, 7); // Final hallway

        // Screens Room Exit Path
        // TODO: make it appear after the player collects the key
        grid.FillFloor(-7, 7, -2, 7);
    }

}
