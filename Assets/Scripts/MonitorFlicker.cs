using UnityEngine;

public class MonitorFlicker : MonoBehaviour
{
    private Material mat;
    [SerializeField] public float minIntensity = 0.5f;
    [SerializeField] public float maxIntensity = 2f;
    [SerializeField] public float flickerSpeed = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Renderer>().material;

        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);

    }

    void Flicker()
    {
        float intensity = Random.Range(minIntensity, maxIntensity);

        mat.SetColor("_EmissionColor", Color.white * intensity);
    }
}
