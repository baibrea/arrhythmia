using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class HeartbeatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle toggle;
    [SerializeField] private AudioSource sound;
    [SerializeField] private Volume volume;
    private Vignette vignette;
    private float bpm;
    private float progress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume.profile.TryGet(out vignette);
        bpm = slider.value;
        vignette.intensity.overrideState = true;
        StartCoroutine(Heartbeat());
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "BPM: " + bpm;
        bpm = slider.value;
        vignette.intensity.value = Mathf.Lerp(0, 0.5f, bpm / 160);
    }

    IEnumerator Heartbeat()
    {
        image.color = new Color(1, 0, 0, 0.5f);
        float interval = 60f / bpm;
        float timer = interval;
        StartCoroutine(HeartbeatSound(interval));
        while (timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
            progress = timer / interval;
            GetComponent<RectTransform>().localScale = new Vector3(progress, progress, progress);
            if (toggle.isOn && progress <= 0.3f)
            {
                image.color = new Color(0, 1, 0, 0.5f);
            }
        }
        StartCoroutine(Heartbeat());
    }

    IEnumerator HeartbeatSound(float time)
    {
        yield return new WaitForSeconds(time - time * 0.25f);
        sound.pitch = Mathf.Lerp(0, 2, bpm / 200);
        sound.PlayOneShot(sound.clip);
    }
    public float getProgress()
    {
        return progress;
    }
    public float getBPM()
    {
        return bpm;
    }

}
