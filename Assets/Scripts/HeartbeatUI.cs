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
    [SerializeField] private GameObject heartObject;
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle toggle;
    [SerializeField] private AudioSource sound;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    private Image heart;
    private GameObject left1;
    private GameObject right1;
    private GameObject left2;
    private GameObject right2;
    private Vignette vignette;
    private float bpm = 60f;
    private float progress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        heart = heartObject.GetComponent<Image>();
        volume.profile.TryGet(out vignette);
        bpm = slider.value;
        vignette.intensity.overrideState = true;
        left1 = Instantiate(left, gameObject.transform);
        left2 = Instantiate(left, gameObject.transform);
        right1 = Instantiate(right, gameObject.transform);
        right2 = Instantiate(right, gameObject.transform);
        StartCoroutine(Heartbeat());
        // StartCoroutine(MoveOutlines());
    }

    void UpdateQueue()
    {
        GameObject tempLeft = left;
        GameObject tempRight = right;
        left = left1;
        right = right1;
        left1 = left2;
        right1 = right2;
        left2 = tempLeft;
        right2 = tempRight;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "BPM: " + bpm + " Progress: " + progress;
        vignette.intensity.value = Mathf.Lerp(0, 0.5f, bpm / 160);
        left1.transform.localPosition = new Vector3(left.transform.localPosition.x - 150, 0, 0);
        left2.transform.localPosition = new Vector3(left.transform.localPosition.x - 300, 0, 0);
        right1.transform.localPosition = new Vector3(right.transform.localPosition.x + 150, 0, 0);
        right2.transform.localPosition = new Vector3(right.transform.localPosition.x + 300, 0, 0);
    }

    IEnumerator Heartbeat()
    {
        heart.color = Color.white;
        float interval = 60f / bpm;
        float timer = interval;
        StartCoroutine(HeartbeatSound(interval));
        Image leftImage = left.GetComponent<Image>();
        Image rightImage = right.GetComponent<Image>();
        Image left1Image = left1.GetComponent<Image>();
        Image right1Image = right1.GetComponent<Image>();
        Image left2Image = left2.GetComponent<Image>();
        Image right2Image = right2.GetComponent<Image>();
        while (timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
            progress = Mathf.Round((timer / interval) * 100f) / 100f;
            left.transform.localPosition = Vector3.Lerp(new Vector3(-150, 0, 0), Vector3.zero, 1 - progress);
            right.transform.localPosition = Vector3.Lerp(new Vector3(150, 0, 0), Vector3.zero, 1 - progress);
            leftImage.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 + left.transform.localPosition.x / 450));
            rightImage.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - right.transform.localPosition.x / 450));
            left1Image.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 + left1.transform.localPosition.x / 450));
            right1Image.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - right1.transform.localPosition.x / 450));
            left2Image.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 + left2.transform.localPosition.x / 450));
            right2Image.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - right2.transform.localPosition.x / 450));
            if (toggle.isOn && (progress <= 0.15f || progress >= 0.85f))
            {
                heart.color = Color.green;
            }
            else
            {
                heart.color = Color.white;
            }
        }
        UpdateQueue();
        StartCoroutine(Pulse(interval * 0.1f));
        StartCoroutine(Heartbeat());
    }

    IEnumerator Pulse(float time)
    {
        float timer = time;
        Transform heartTransform = heart.transform;

        float endTime = time * 0.7f;
        float duration = time - endTime;
        while (timer > endTime)
        {
            float t = 1 - (timer - endTime) / duration;
            heartTransform.localScale = Vector3.one * Mathf.Lerp(1, 1.35f, t);
            timer -= Time.deltaTime;
            yield return null;
        }
        endTime = time * 0.5f;
        duration = time - endTime;
        while (timer > endTime)
        {
            float t = 1 - (timer - endTime) / duration;
            heartTransform.localScale = Vector3.one * Mathf.Lerp(1.35f, 1.1f, t);
            timer -= Time.deltaTime;
            yield return null;
        }
        endTime = time * 0.3f;
        duration = time - endTime;
        while (timer > endTime)
        {
            float t = 1 - (timer - endTime) / duration;
            heartTransform.localScale = Vector3.one * Mathf.Lerp(1.1f, 1.25f, t);
            timer -= Time.deltaTime;
            yield return null;
        }
        endTime = 0;
        duration = time - endTime;
        while (timer > endTime)
        {
            float t = 1 - (timer - endTime) / duration;
            heartTransform.localScale = Vector3.one * Mathf.Lerp(1.25f, 1f, t);
            timer -= Time.deltaTime;
            yield return null;
        }
        heartTransform.localScale = Vector3.one;
    }

    IEnumerator MoveOutlines()
    {
        GameObject leftCopy = Instantiate(left, gameObject.transform);
        GameObject rightCopy = Instantiate(right, gameObject.transform);
        while (progress > 0f)
        {
            yield return null;
            left.transform.localPosition = Vector3.Lerp(new Vector3(-500, 0, 0), Vector3.zero, 1 - progress);
            right.transform.localPosition = Vector3.Lerp(new Vector3(500, 0, 0), Vector3.zero, 1 - progress);
        }
        Destroy(leftCopy);
        Destroy(rightCopy);
    }

    IEnumerator HeartbeatSound(float time)
    {
        yield return new WaitForSeconds(time - time * 0.25f);
        sound.pitch = Mathf.Lerp(0, 2, bpm / 200);
        sound.PlayOneShot(sound.clip, 2);
    }

    public float getProgress()
    {
        return progress;
    }

    public float getBPM()
    {
        return bpm;
    }

    public void setBPM(float value) 
    {
        bpm = value;
    }
}
