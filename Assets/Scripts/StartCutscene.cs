using DigitalWorlds.StarterPackage3D;
using UnityEngine;
using UnityEngine.Video;

public class StartCutscene : MonoBehaviour
{
    private VideoPlayer vp;
    private ChangeScene changeScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        changeScene = GetComponent<ChangeScene>();
        vp.loopPointReached += LoopPointReached;
    }

    private void LoopPointReached(VideoPlayer source)
    {
        changeScene.LoadSceneByIndex(1);
    }
}
