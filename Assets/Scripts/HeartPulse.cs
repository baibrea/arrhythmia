using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartPulse : MonoBehaviour
{
    [SerializeField] HeartbeatUI heartbeat;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(Pulse());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Pulse()
    {
        yield return new WaitForSeconds(60f / heartbeat.getBPM());
        animator.SetTrigger("Pulse");
        StartCoroutine(Pulse());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
