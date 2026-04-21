using DigitalWorlds.Dialogue;
using System;
using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Closet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer prompt;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Light playerLight;
    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private GameObject heartbeatSystem;
    [SerializeField] private GameObject rhythmUI;
    [SerializeField] private Transform notes;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject window;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private MonsterPath monsterPath;

    [Header("Timing Displays")]
    [SerializeField] private Transform timingParent;
    [SerializeField] private GameObject greatPrefab;
    [SerializeField] private GameObject goodPrefab;
    [SerializeField] private GameObject okayPrefab;
    [SerializeField] private GameObject missPrefab;


    public InputActionAsset asset;
    InputActionMap inputActions;
    InputAction interact;
    InputAction move;
    private float bpm;
    private bool canInteract = true;
    private bool insideTrigger = false;
    private int misses = 0;
    private RectTransform windowRect;
    private AudioSource pulseSource;
    private DialogueTrigger dialogueTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = asset.FindActionMap("Move");
        interact = inputActions.FindAction("Interact");
        move = inputActions.FindAction("WASD");
        inputActions.Enable();
        windowRect = window.GetComponent<RectTransform>();
        pulseSource = GetComponent<AudioSource>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        rhythmUI.SetActive(false);

        prompt.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        whileInside();
        bpm = heartbeat.getBPM();
        if (prompt.enabled)
        {
            if (interact.WasPressedThisFrame() && canInteract)
            {
                StartCoroutine(InteractCooldown(0.3f));
                prompt.enabled = false;
                playerSprite.enabled = false;
                playerLight.enabled = false;
                playerMove.toggleCloset(true);
                heartbeat.stopRunning();
                rhythmUI.SetActive(true);
                misses = 0;
                StartCoroutine(RhythmGame(0.5f));
            }
        }
    }

    IEnumerator RhythmGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        int rand = Random.Range(1, 5);
        Vector2 direction;
        GameObject currentNote = Instantiate(notePrefab, notes, false);
        Image noteImage = currentNote.GetComponent<Image>();
        TextMeshProUGUI key = currentNote.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        Color color;

        // Left (A)
        if (rand == 1)
        {
            color = key.color = Color.red;
            key.text = "A";
            direction = new Vector2(-1, 0);
        }
        // Right (D)
        else if (rand == 2)
        {
            color = key.color = Color.green;
            key.text = "D";
            direction = new Vector2(1, 0);
        }
        // Up (W)
        else if (rand == 3)
        {
            color = key.color = Color.blue;
            key.text = "W";
            direction = new Vector2(0, 1);
        }
        // Down (S)
        else
        {
            color = key.color = Color.yellow;
            key.text = "S";
            direction = new Vector2(0, -1);
        }
        color.a = 0.5f;
        noteImage.color = color;

        float time = 240f / bpm;
        float period = time;

        int next = Random.Range(1, 6);
        bool sent = false;

        float leftBoundary = windowRect.position.x - (windowRect.rect.width / 2);
        float rightBoundary = windowRect.position.x + (windowRect.rect.width / 2);
        float multiplier = 1f;

        while (time > 0f)
        {
            currentNote.transform.localPosition = Vector3.Lerp(new Vector3(650, 0, 0), new Vector3(2400f, 0, 0), time / period);
            time -= Time.deltaTime;
            yield return null;

            if (currentNote.transform.localPosition.x < 2400f - (200f * next) && !sent)
            {
                sent = true;
                StartCoroutine(RhythmGame(0));
            }

            if (currentNote.transform.position.x > leftBoundary && currentNote.transform.position.x < rightBoundary)
            {
                if (move.ReadValue<Vector2>() == direction && move.WasPressedThisFrame())
                {
                    pulseSource.pitch = Mathf.Lerp(0.9f, 1.4f, (rand - 1f) / 3f);
                    pulseSource.PlayOneShot(pulseSource.clip);
                    if (heartbeat.getBPM() <= 60f)
                    {
                        heartbeat.setBPM(60f);
                    }

                    if (currentNote.transform.position.x > leftBoundary + 75 && currentNote.transform.position.x < rightBoundary - 75)
                    {
                        Instantiate(greatPrefab, timingParent, false);
                        Animator noteAnimator = currentNote.GetComponent<Animator>();
                        currentNote.transform.position = windowRect.position;
                        currentNote.transform.SetParent(timingParent, true);
                        noteAnimator.SetTrigger("Pop");
                        multiplier = 0.975f;
                        heartbeat.setBPM(heartbeat.getBPM() * multiplier);
                        Destroy(currentNote, 0.2f);
                        yield break;
                    }
                    else if (currentNote.transform.position.x > leftBoundary + 50 && currentNote.transform.position.x < rightBoundary - 50)
                    {
                        Instantiate(goodPrefab, timingParent, false);
                        multiplier = 0.98f;
                    }
                    else
                    {
                        Instantiate(okayPrefab, timingParent, false);
                        multiplier = 0.99f;
                    }
                    heartbeat.setBPM(heartbeat.getBPM() * multiplier);
                    
                    Destroy(currentNote);
                    yield break;
                } else if (move.ReadValue<Vector2>() != direction && move.WasPressedThisFrame())
                {
                    Instantiate(missPrefab, timingParent, false);
                    pulseSource.pitch = 0.5f;
                    pulseSource.PlayOneShot(pulseSource.clip);
                    Destroy(currentNote);
                    misses += 1;
                    yield break;
                }
            }

        }
        Instantiate(missPrefab, timingParent, false);
        pulseSource.pitch = 0.5f;
        pulseSource.PlayOneShot(pulseSource.clip);
        Destroy(currentNote);
        misses += 1;
    }

    IEnumerator InteractCooldown(float seconds)
    {
        if (canInteract)
        {
            canInteract = false;
            yield return new WaitForSeconds(seconds);
            canInteract = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        prompt.enabled = true;
        insideTrigger = true;
    }

    private void whileInside()
    {
        if (interact.WasPressedThisFrame() && !prompt.enabled && canInteract && insideTrigger)
        {
            StopAllCoroutines();
            StartCoroutine(InteractCooldown(0.3f));
            prompt.enabled = true;
            playerSprite.enabled = true;
            playerLight.enabled = true;
            foreach (Transform child in notes)
            {
                Destroy(child.gameObject);
            }
            rhythmUI.SetActive(false);
            heartbeat.startRunning();
        }
        if (misses == 3)
        {
            monsterPath.AlertMonster();
            misses = 4;
            dialogueTrigger.TriggerDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        insideTrigger = false;
        canInteract = true;
        prompt.enabled = false;
        playerSprite.enabled = true;
        misses = 0;
        playerLight.enabled = true;
        playerMove.toggleCloset(false);
        StopAllCoroutines();
    }
}
