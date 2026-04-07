using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GridManager grid;

    [SerializeField] private GameObject space1;
    [SerializeField] private GameObject space2;
    [SerializeField] private GameObject parent;
    [SerializeField] private bool generatePrefabs = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildTutorial();
    }

    void BuildTutorial()
    {
        grid.FillFloor(-4, -3, 4, 3);

    }

}
