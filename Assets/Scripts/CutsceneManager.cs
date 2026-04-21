using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private MonsterPath monster;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterTransform;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip chaseMusic;
    private bool playing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        playing = true;
        audioSource.clip = chaseMusic;
        audioSource.Play();
        heartbeat.stopRunning();
        monster.ForceStun(true);
        monster.ChangePositon(-13, 10);
        monster.AlertMonster();
        cam.Follow = monsterTransform;
        yield return new WaitForSeconds(3f);
        cam.Follow = playerTransform;
        yield return new WaitForSeconds(0.5f);
        if (heartbeat.getBPM() < 100)
        {
            heartbeat.setBPM(100);
        }
        heartbeat.startRunning(0f);
        monster.ForceStun(false);
        while (playing)
        {
            monster.AlertMonster();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StopCutscene()
    {
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
