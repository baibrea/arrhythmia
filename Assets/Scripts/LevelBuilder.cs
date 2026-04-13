using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildTutorial();
    }

    void BuildTutorial()
    {
        grid.FillFloor(-4, -3, 4, 3);
        grid.SetTile(-5, 0, "door");

        grid.FillFloor(-21, -3, -6, 3);
        grid.FillFloor(-19, 4, -18, 10);
    }

}
