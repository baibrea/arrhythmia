using DigitalWorlds.Dialogue;
using DigitalWorlds.StarterPackage3D;
using System.Collections;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCutscene : MonoBehaviour
{

    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterTransform;
    [SerializeField] private Transform closetTransform;
    [SerializeField] private Transform keyTransform;
    [SerializeField] private CinemachineCamera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void StartCutscene()
    {
        cam.Follow = keyTransform;
    }

    public void NextScene()
    {
        if (cam.Follow == keyTransform)
        {
            cam.Follow = monsterTransform;
        }
        else if (cam.Follow == monsterTransform)
        {
            cam.Follow = closetTransform;
        } else if (cam.Follow == closetTransform)
        {
            cam.Follow = playerTransform;
            heartbeat.startRunning(0.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
