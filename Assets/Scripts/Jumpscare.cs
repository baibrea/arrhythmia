using DigitalWorlds.StarterPackage3D;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private ChangeScene ChangeScene;
    private RectTransform rectTransform;
    private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scare()
    {
        StartCoroutine(ScareRoutine());
    }

    IEnumerator ScareRoutine()
    {
        image.enabled = true;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float size = Mathf.Lerp(50f, 2000f, t / duration);
            rectTransform.sizeDelta = new Vector2(size, size);

            yield return null;
        }

        ChangeScene.LoadCurrentScene();
    }
}
