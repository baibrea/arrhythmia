// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Logan Kemper

using UnityEngine;

namespace DigitalWorlds.StarterPackage3D
{
    /// <summary>
    /// Add to a light to flicker its intensity up and down.
    /// </summary>
    public class FlickeringLight : MonoBehaviour
    {
        [SerializeField] private Light m_light;
        [SerializeField] private float frequency = 1f;

        private float randomSeed;

        private float defaultIntensity;

        private void Start()
        {
            if (m_light == null) m_light = GetComponent<Light>();
            randomSeed = Random.Range(0f, 65535f);

            // Capture the intensity you set in the Inspector
            defaultIntensity = m_light.intensity;
        }

        private void Update()
        {
            if (m_light == null) return;

            float noise = Mathf.PerlinNoise(randomSeed, Time.time * frequency);

            // Flicker between 10% and 150% of the light's original brightness
            float flickerFactor = Mathf.Lerp(0.1f, 1.5f, noise);

            m_light.intensity = defaultIntensity * flickerFactor;
        }
    }
}