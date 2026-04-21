using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DigitalWorlds.StarterPackage3D;
using DigitalWorlds.Dialogue;
using System;

public class PlayerMove : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private GameObject heartbeatSystem;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GridManager grid;
    [SerializeField] private TextMeshProUGUI indicator;

    [Header("BPM Settings")]
    [SerializeField] private float threshold = 0.2f;
    [SerializeField] private float increaseAmount = 5f;

    [Header("Visual References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Transform facingLight;
    [SerializeField] private Flashlight flashlight;

    [Header("Player Settings")]
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    public InputActionAsset asset;
    InputActionMap inputActions;
    InputAction move;
    InputAction idle;
    InputAction flash;
    private Vector2 direction;
    private float speed;
    private Inventory inventory;
    private (int, int) position = (0, 0);
    private (int, int) prevPosition = (0, 0);
    private float lastProgress;
    private bool playerInputted = false;
    private bool firstBeat = true;
    private bool currBeatFailed = false;
    private (int, int) facing = (-1, -1);
    private bool monsterWait = false;
    private bool inCloset = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = asset.FindActionMap("Move");
        move = inputActions.FindAction("WASD");
        idle = inputActions.FindAction("Idle");
        flash = inputActions.FindAction("Flashlight");
        inputActions.Enable();

        inventory = FindFirstObjectByType<Inventory>();
        position = (startX, startY);
        prevPosition = position;
        gameObject.transform.position = new Vector3(startX, 0.35f, startY);


        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        speed = 120 / heartbeat.getBPM();
        checkMiss();
    }

    IEnumerator Move()
    {
        if (!heartbeat.checkRunning())
        {
            monsterWait = false;
        }
        else
        {
            monsterWait = true;
        }
            direction = move.ReadValue<Vector2>();
        bool idlePressed = false;

        while (direction != Vector2.zero)
        {
            direction = move.ReadValue<Vector2>();
            yield return null;
        }
        while ((direction == Vector2.zero && !idlePressed && !flash.WasPressedThisFrame()) || !heartbeat.checkRunning())
        {
            direction = move.ReadValue<Vector2>();
            idlePressed = idle.WasPressedThisFrame();
            yield return null;
        }
        if (
            ((heartbeat.getProgress() <= threshold / 2f || heartbeat.getProgress() >= 1 - threshold / 2f)))
        {
            if (!heartbeat.checkRunning())
            {
                idlePressed = true;
            }
            // Check for idle
            if (idlePressed)
            {
                if (indicator != null)
                {
                    indicator.text = "IDLE SUCCESS";
                    indicator.color = Color.yellow;
                }
                playerInputted = true;
            } 
            else if (flash.WasPressedThisFrame())
            {
                Debug.Log("Flashlight Pressed");
                flashlight.Flash();
                playerInputted = true;
            }
            // Move player
            else
            {
                prevPosition = position;
                animator.SetFloat("hopMult", 0.625f + heartbeat.getBPM() / 160f);
                animator.SetTrigger("Hop");
                if (direction.y != 0)
                {
                    int sign = (int)Mathf.Sign(direction.y);
                    facing.Item2 = sign;
                    updateFacingLightY();

                    if (grid.GetTile(position.Item1, position.Item2 + 1 * sign) == "floor")
                    {
                        Vector3 target = new Vector3(position.Item1, transform.position.y, position.Item2 + 1 * sign);
                        transform.position = Vector3.MoveTowards(transform.position, target, 2.5f);
                        position = (position.Item1, position.Item2 + 1 * sign);
                    }
                    // For door tiles, ensure door tile exists at coordinates and try unlocking it if it does
                    else if (grid.GetTile(position.Item1, position.Item2 + 1 * sign) == "door")
                    {
                        Lock3D door = grid.CheckDoor(position.Item1, position.Item2 + 1 * sign);
                        if (door != null)
                        {
                            door.TryUnlock(inventory, direction);
                        }
                    }
                }
                else if (direction.x != 0)
                {
                    int sign = (int)Mathf.Sign(direction.x);

                    if (facing.Item1 != sign)
                    {
                        playerSprite.flipX = !playerSprite.flipX;
                        facing.Item1 = sign;
                    }
                    updateFacingLightX();
                    Debug.Log(grid.GetTile(-5, 0));
                    if (grid.GetTile(position.Item1 + 1 * sign, position.Item2) == "floor")
                    {
                        Vector3 target = new Vector3(position.Item1 + 1 * sign, transform.position.y, position.Item2);
                        transform.position = Vector3.MoveTowards(transform.position, target, 2.5f);
                        position = (position.Item1 + 1 * sign, position.Item2);
                    }
                    // For door tiles, ensure door tile exists at coordinates and try unlocking it if it does
                    else if (grid.GetTile(position.Item1 + 1 * sign, position.Item2) == "door")
                    {
                        Lock3D door = grid.CheckDoor(position.Item1 + 1 * sign, position.Item2);
                        if (door != null)
                        {
                            Debug.Log("hi");
                            door.TryUnlock(inventory, direction);
                        }
                    }
                }
                if (indicator != null)
                {
                    indicator.text = "MOVE SUCCESS";
                    indicator.color = Color.green;
                }

                playerInputted = true;
            }
            monsterWait = false;
        }
        else
        {
            if (indicator != null)
            {
                indicator.text = "MOVE FAIL";
                indicator.color = Color.red;
            }
            // Increase BPM
            if (heartbeat.getBPM() < 160 && heartbeat.checkRunning())
            {
                if (!DialogueManager.AnyDialogueActive)
                {
                    heartbeat.setBPM(heartbeat.getBPM() + increaseAmount);
                }
                cameraShake.ShakeCamera();
            }
            currBeatFailed = true;
            monsterWait = false;
        }
        yield return new WaitForSeconds(speed / 3);
        StartCoroutine(Move());
    }

    void updateFacingLightY()
    {
        facingLight.rotation = Quaternion.Euler(0f, 90f * (facing.Item2 - 1f), 0f);
    }

    void updateFacingLightX()
    {
        facingLight.rotation = Quaternion.Euler(0f, 90f * facing.Item1, 0f);
    }

    // Check if the player missed a beat
    void checkMiss()
    {
        float currentProgress = heartbeat.getProgress();

        // When the beat threshold ends, checks if the player inputted
        if (currentProgress < (1 - threshold / 2f) && lastProgress >= (1 - threshold / 2f) && heartbeat.checkRunning())
        {
            if (firstBeat)
            {
                firstBeat = false;
                lastProgress = heartbeat.getProgress();
                return;
            }
            if (!playerInputted && !currBeatFailed)
            {
                // Increase BPM
                if (heartbeat.getBPM() < 160)
                {
                    cameraShake.ShakeCamera();
                    if (!DialogueManager.AnyDialogueActive)
                    {
                        heartbeat.setBPM(heartbeat.getBPM() + increaseAmount);
                    }
                }
            }
            playerInputted = false;
            currBeatFailed = false;
            monsterWait = false;
        }
        lastProgress = currentProgress;
    }

    public (int, int) GetPreviousPosition()
    {
        return prevPosition;
    }

    public (int, int) GetCurrentPosition()
    {
        return position;
    }

    public bool getMonsterWait()
    {
        return monsterWait;
    }

    public void setFirstBeat(bool i)
    {
        firstBeat = i;
    }

    public void toggleCloset(bool b)
    {
        inCloset = b;
    }

    public bool checkCloset()
    {
        return inCloset;
    }
}
