using DigitalWorlds.StarterPackage3D;
using UnityEngine;
using UnityEngine.Video;

public class EndCutscene : MonoBehaviour
{
    [SerializeField] private VideoClip clip;
    private VideoPlayer vp;
    private ChangeScene changeScene;
    private int counter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        changeScene = GetComponent<ChangeScene>();
        vp.loopPointReached += LoopPointReached;
    }

    private void LoopPointReached(VideoPlayer source)
    {
        counter++;
        if (counter == 2)
        {
            vp.playbackSpeed = 0.15f;
            vp.clip = clip;
            vp.isLooping = false;
        } else if (counter == 3)
        {
            changeScene.LoadSceneByName("Credits");
        }
    }
}
