using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildTutorial();
        BuildLevels();

        // TODO: call this when exit doors are unlocked
        BuildFinalChase();
    }

    void BuildTutorial()
    {
        grid.FillFloor(-4, -3, 4, 3);
        grid.SetTile(-5, 0, "door");
    }

    void BuildLevels()
    {
        // Hub
        grid.FillFloor(-21, 0, -6, 3);

        // Hallways
        grid.FillFloor(-19, 4, -18, 10);
        grid.FillFloor(-9, 4, -8, 10);

        // Screens Room
        grid.FillFloor(-9, 11, -2, 18); // Entry Room
        grid.FillWall(-7, 11, -7, 13); // Wall along entry hallway
        grid.FillWall(-5, 13, -3, 13); // Bottom wall
        grid.FillWall(-4, 16, -4, 18); // Top wall
        grid.FillFloor(-1, 17, 0, 18); // Walkway to 2nd room
        grid.SetTile(-9, 18, "wall");
        grid.SetTile(-6, 11, "wall");
        grid.SetTile(-2, 11, "wall");

        grid.FillFloor(1, 12, 10, 22); // 2nd room floor
        grid.FillWall(3, 19, 4, 19); // 2nd room, top left wall
        grid.FillWall(7, 18, 7, 20); // 2nd room, top right wall
        grid.SetTile(8, 18, "wall");
        grid.SetTile(1, 22, "wall");
        grid.SetTile(10, 22, "wall");

        grid.FillWall(1, 15, 3, 16); // 2nd room, left wall
        grid.FillWall(6, 13, 10, 14); // 2nd room, bottom right wall
        grid.SetTile(1, 14, "wall");

        grid.FillFloor(9, 8, 10, 11); // Walkway to final hallway
        grid.FillFloor(-1, 7, 10, 7); // Final hallway
    }

    public void BuildShortcut()
    {
        grid.FillFloor(-7, 7, -2, 7);
    }

    public void BuildFinalChase()
    {
        grid.FillFloor(-14, 4, -13, 40);

        grid.SetTile(-13, 7, "wall");
        grid.SetTile(-13, 8, "wall");
        grid.SetTile(-13, 14, "wall");
        grid.SetTile(-13, 22, "wall");
        grid.SetTile(-13, 23, "wall");
        grid.SetTile(-13, 31, "wall");

        grid.SetTile(-14, 11, "wall");
        grid.SetTile(-14, 18, "wall");
        grid.SetTile(-14, 19, "wall");
        grid.SetTile(-14, 27, "wall");
        grid.SetTile(-14, 35, "wall");

    }

}