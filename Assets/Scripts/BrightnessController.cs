using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume volume;
    [SerializeField] private Slider brightnessSlider;

    private LiftGammaGain liftGammaGain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (volume.profile.TryGet<LiftGammaGain>(out liftGammaGain))
        {
            float savedBrightness = PlayerPrefs.GetFloat("BrightnessPrefs", 1f);
            if (brightnessSlider != null)
            {
                brightnessSlider.value = savedBrightness;
            }
            ApplyBrightness(PlayerPrefs.GetFloat("BrightnessPrefs", 1f));
            Debug.Log("LiftGammaGain found and brightness applied:" + PlayerPrefs.GetFloat("BrightnessPrefs", 1f));
        }
        else
        {
            Debug.LogError("LiftGammaGain not found in the Volume profile.");
        }
    }

    public void SetBrightness(float value)
    {
        Debug.Log("Brightness slider value changed: " + value);

        ApplyBrightness(value);
        PlayerPrefs.SetFloat("BrightnessPrefs", value);
    }

    private void ApplyBrightness(float value)
    {
        if (liftGammaGain == null) return;
        Debug.Log("Applying brightness: " + value);

        Vector4 newLGG = new Vector4(1f, 1f, 1f, value);
        liftGammaGain.gamma.Override(newLGG);
        liftGammaGain.gain.Override(newLGG);
    }
}
