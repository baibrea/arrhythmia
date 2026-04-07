using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Closet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer prompt;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Light playerLight;
    [SerializeField] private HeartbeatUI heartbeat;
    [SerializeField] private GameObject heartbeatSystem;
    [SerializeField] private GameObject rhythmUI;
    public InputActionAsset asset;
    InputActionMap inputActions;
    InputAction interact;
    private float bpm;
    private bool canInteract = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = asset.FindActionMap("Move");
        interact = inputActions.FindAction("Interact");
        inputActions.Enable();
        rhythmUI.SetActive(false);

        prompt.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (prompt.enabled)
        {
            if (interact.WasPressedThisFrame() && canInteract)
            {
                prompt.enabled = false;
                playerSprite.enabled = false;
                playerLight.enabled = false;
                heartbeat.stopRunning();
                StartCoroutine(InteractCooldown(0.5f));
                StartCoroutine(RhythmGame());
            }
        }
    }

    IEnumerator RhythmGame()
    {
        rhythmUI.SetActive(true);
        yield return null;
    }

    IEnumerator InteractCooldown(float seconds)
    {
        canInteract = false;
        yield return new WaitForSeconds(seconds);
        canInteract = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        prompt.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (interact.WasPressedThisFrame() && prompt.enabled == false && canInteract)
        {
            prompt.enabled = true;
            playerSprite.enabled = true;
            playerLight.enabled = true;
            rhythmUI.SetActive(false);
            heartbeat.startRunning();
            StartCoroutine(InteractCooldown(0.5f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        prompt.enabled = false;
        playerSprite.enabled = true;
        playerLight.enabled = true;
    }
}
