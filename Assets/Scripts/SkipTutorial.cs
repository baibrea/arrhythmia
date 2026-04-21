using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    public static bool skipped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Skip()
    {
        skipped = true;
    }

    public void DoNotSkip()
    {
        skipped = false;
    }
}
