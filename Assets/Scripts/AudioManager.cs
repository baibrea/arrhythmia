using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider heartbeatSlider;

    const float defaultVolume = 0.75f;

    void Start()
    {
        // Load saved values and apply volumes
        LoadVolume(musicSlider, "MusicVolume", "MusicPref");
        LoadVolume(sfxSlider, "SFXVolume", "SFXPref");
        LoadVolume(heartbeatSlider, "HeartbeatVolume", "HeartbeatPref");

        // Add listeners
        SetupSlider(musicSlider, "MusicVolume", "MusicPref");
        SetupSlider(sfxSlider, "SFXVolume", "SFXPref");
        SetupSlider(heartbeatSlider, "HeartbeatVolume", "HeartbeatPref");
    }

    void SetupSlider(Slider slider, string mixerParam, string prefKey)
    {
        // Live update while dragging
        slider.onValueChanged.AddListener(value => ApplyVolume(mixerParam, value));

        // Save when player releases slider
        var eventTrigger = slider.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null) eventTrigger = slider.gameObject.AddComponent<EventTrigger>();

        // Add EndDrag event
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
        entry.callback.AddListener((data) => SaveVolume(slider.value, prefKey));
        eventTrigger.triggers.Add(entry);
    }

    void ApplyVolume(string parameter, float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        float dB = Mathf.Log10(value) * 20;
        mixer.SetFloat(parameter, dB);
    }

    void LoadVolume(Slider slider, string parameter, string prefKey)
    {
        float value = PlayerPrefs.GetFloat(prefKey, defaultVolume);
        slider.value = value;
        ApplyVolume(parameter, value);
    }

    void SaveVolume(float value, string prefKey)
    {
        PlayerPrefs.SetFloat(prefKey, value);
        PlayerPrefs.Save();
    }
}