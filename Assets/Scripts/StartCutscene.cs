using DigitalWorlds.StarterPackage3D;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.Video;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private VideoClip scene2;
    private VideoPlayer vp;
    private ChangeScene changeScene;
    private int count = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        changeScene = GetComponent<ChangeScene>();
        vp.loopPointReached += LoopPointReached;
    }

    private void LoopPointReached(VideoPlayer source)
    {
        count += 1;
        if (count == 2)
        {
            vp.clip = scene2;
        } else if (count == 4) {
            changeScene.LoadSceneByIndex(1);
        }
    }
}
