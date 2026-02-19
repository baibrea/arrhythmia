using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private TextMeshProUGUI indicator;
    [SerializeField] private float threshold = 0.2f;
    public InputActionAsset asset;
    InputActionMap inputActions;
    InputAction move;
    private Vector2 direction;
    private float speed;
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
            (heartbeat.getProgress() <= threshold / 2f || heartbeat.getProgress() >= 1 - threshold / 2f)
            && (direction.x == 0 || direction.y == 0)
            )
        {
            Vector3 target = new Vector3(transform.position.x + direction.x * 5, transform.position.y, transform.position.z + direction.y * 5);
            transform.position = Vector3.MoveTowards(transform.position, target, 2.5f);
            indicator.text = "MOVE SUCCESS";
            indicator.color = Color.green;
        }
        else
        {
            indicator.text = "MOVE FAIL";
            indicator.color = Color.red;
            cameraShake.ShakeCamera();
        }
        yield return new WaitForSeconds(speed / 3);
        StartCoroutine(Move());
    }

}
