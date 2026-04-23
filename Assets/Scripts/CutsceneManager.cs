using DigitalWorlds.StarterPackage3D;
using System.Collections;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{

    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private MonsterPath monster;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform monsterTransform;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip chaseMusic;
    [SerializeField] private AudioClip transitionSound;
    [SerializeField] private AudioClip exitSound;
    [SerializeField] private Image screen;
    [SerializeField] ChangeScene changeScene;
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
        audioSource.PlayOneShot(transitionSound);
        playing = true;
        heartbeat.stopRunning();
        monster.ForceStun(true);
        monster.ChangePositon(-13, 10);
        monster.AlertMonster();
        cam.Follow = monsterTransform;
        yield return new WaitForSeconds(2f);
        audioSource.clip = chaseMusic;
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
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
        audioSource.Stop();
        heartbeat.stopRunning();
        monster.ForceStun(true);
        audioSource.clip = exitSound;
        audioSource.Play();
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        float duration = 2.5f;
        float timer = duration;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            screen.color = new Color(1, 1, 1, 1f - (timer / duration));
        }
        yield return new WaitForSeconds(1.2f);
        changeScene.LoadSceneByName("Title Screen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
