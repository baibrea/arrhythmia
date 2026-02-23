using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;
    private CinemachineFollow cam;

    private void Start()
    {
        cam = GetComponent<CinemachineFollow>();
    }
    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float timer = shakeDuration;
        Vector3 initialPos = cam.FollowOffset;
        while (timer > 0f)
        {
            Vector3 offset = Random.insideUnitSphere * shakeAmount;
            cam.FollowOffset = initialPos + offset;
            timer -= Time.deltaTime;
            yield return null;
            yield return null;
        }
        cam.FollowOffset = initialPos;
    }

}
