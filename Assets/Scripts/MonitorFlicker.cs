using UnityEngine;
using System.Collections;

public class MonitorFlicker : MonoBehaviour
{
    [SerializeField] private Transform flashOnMissGroup;
    [SerializeField] private Transform normalGroup;

    [SerializeField] public float minIntensity = 0.5f;
    [SerializeField] public float maxIntensity = 2f;

    [SerializeField] private float missFlashIntensity = 4f;
    [SerializeField] private float missFlashDuration = 0.1f;

    private Renderer[] normalRenderers;
    private Renderer[] flashOnMissRenderers;
    private float[] minIntensities;
    private float[] maxIntensities;

    private bool isRed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        normalRenderers = new Renderer[normalGroup.childCount];
        flashOnMissRenderers = new Renderer[flashOnMissGroup.childCount];

        minIntensities = new float[normalGroup.childCount];
        maxIntensities = new float[normalGroup.childCount];

        for (int i = 0; i < normalGroup.childCount; i++)
        {
            normalRenderers[i] = normalGroup.GetChild(i).GetComponent<Renderer>();

            minIntensities[i] = Random.Range(-2, 2f);
            maxIntensities[i] = Random.Range(0.7f, 2f);
        }

        for (int i = 0; i < flashOnMissGroup.childCount; i++)
        {
            flashOnMissRenderers[i] = flashOnMissGroup.GetChild(i).GetComponent<Renderer>();
        }

        InvokeRepeating(nameof(Flicker), 0f, 0.1f);
    }

    void Flicker()
    {
        for (int i = 0; i < normalRenderers.Length; i++)
        {
            Renderer r = normalRenderers[i];
            if (r != null) {
                float intensity = Random.Range(minIntensities[i], maxIntensities[i]);
                r.material.SetColor("_EmissionColor", Color.white * intensity);
            }
        }
    }

    IEnumerator FlickerRed()
    {
        isRed = true;
        foreach (Renderer r in flashOnMissRenderers)
        {
            if (r != null)
            {
                r.material.SetColor("_EmissionColor", Color.black * missFlashIntensity);
            }
        }

        yield return new WaitForSeconds(missFlashDuration);

        foreach (Renderer r in flashOnMissRenderers)
        {
            if (r != null)
            {
                r.material.SetColor("_EmissionColor", Color.white);
            }
        }

        isRed = false;
    }

    public void TriggerRedFlash()
    {
        if (isRed) return;
        
        StartCoroutine(FlickerRed());
    }
}
