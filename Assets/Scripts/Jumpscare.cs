using DigitalWorlds.StarterPackage3D;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private ChangeScene ChangeScene;
    private VideoPlayer vp;
    private RawImage image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<RawImage>();
        image.enabled = false;
        vp = GetComponent<VideoPlayer>();
        vp.Prepare();
        vp.loopPointReached += OnVideoEnd;
    }

    public void Scare()
    {
        image.enabled = true;
        vp.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Jumpscare!");
        vp.Stop();
        ChangeScene.LoadCurrentScene();
    }
}
