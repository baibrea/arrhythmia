using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GridManager grid;
    [SerializeField] private TextMeshProUGUI indicator;
    [SerializeField] private float threshold = 0.2f;
    [SerializeField] private float increaseAmount = 5f;
    public InputActionAsset asset;
    InputActionMap inputActions;
    InputAction move;
    private Vector2 direction;
    private float speed;
    private (int, int) position = (0, 0);
    private float lastProgress;
    private bool playerInputted = false;
    private bool firstBeat = true;
    private bool currBeatFailed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = asset.FindActionMap("Move");
        move = inputActions.FindAction("WASD");
        inputActions.Enable();

        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        speed = 120 / heartbeat.getBPM();
        checkMiss();
    }

    IEnumerator Move()
    {
        direction = move.ReadValue<Vector2>();
        while (direction != Vector2.zero)
        {
            direction = move.ReadValue<Vector2>();
            yield return null;
        }
        while (direction == Vector2.zero)
        {
            direction = move.ReadValue<Vector2>();
            yield return null;
        }
        if (
            (heartbeat.getProgress() <= threshold / 2f || heartbeat.getProgress() >= 1 - threshold / 2f))
        {
            if (direction.y != 0)
            {
                int sign = (int) Mathf.Sign(direction.y);
                if (grid.gridObject.CheckSpace(position.Item1, position.Item2 + 1 * sign) == 1)
                {
                    Vector3 target = new Vector3(position.Item1, transform.position.y, position.Item2 + 1 * sign);
                    transform.position = Vector3.MoveTowards(transform.position, target, 2.5f);
                    position = (position.Item1, position.Item2 + 1 * sign);
                }
            } else if (direction.x != 0)
            {
                int sign = (int)Mathf.Sign(direction.x);
                if (grid.gridObject.CheckSpace(position.Item1 + 1 * sign, position.Item2) == 1)
                {
                    Vector3 target = new Vector3(position.Item1 + 1 * sign, transform.position.y, position.Item2);
                    transform.position = Vector3.MoveTowards(transform.position, target, 2.5f);
                    position = (position.Item1 + 1 * sign, position.Item2);
                }
            }

            if (indicator != null)
            {
                indicator.text = "MOVE SUCCESS";
                indicator.color = Color.green;
            }

            playerInputted = true;
        }
        else
        {
            if (indicator != null) 
            {
                indicator.text = "MOVE FAIL";
                indicator.color = Color.red;
            }
            // Increase BPM
            if (heartbeat.getBPM() < 160) 
            {
                heartbeat.setBPM(heartbeat.getBPM() + increaseAmount);
                cameraShake.ShakeCamera();
            }
            currBeatFailed = true;
        }
        yield return new WaitForSeconds(speed / 3);
        StartCoroutine(Move());
    }

    // Checks if the player missed a beat
    void checkMiss() 
    {
        float currentProgress = heartbeat.getProgress();

        // When beat threshold ends, checks if player inputted
        if (currentProgress < (1 - threshold / 2f) && lastProgress >= ( 1 - threshold / 2f)) 
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
                    heartbeat.setBPM(heartbeat.getBPM() + increaseAmount);
                    cameraShake.ShakeCamera();
                }
            }
            playerInputted = false;
            currBeatFailed = false;
        }
        lastProgress = currentProgress;
    }

}
