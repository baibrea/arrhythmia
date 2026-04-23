using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource echo;
    private RectTransform rect;
    [SerializeField] GameObject title;
    [SerializeField] private AudioClip song;
    void Start()
    {
        echo = GetComponent<AudioSource>();
        rect = GetComponent<RectTransform>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(Credits());
    }

    IEnumerator Credits()
    {
        yield return new WaitForSeconds(1f);
        echo.Play();
        title.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        float duration = 8f;
        float timer = duration;
        echo.clip = song;
        echo.Play();
        while (timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
            rect.localPosition = new Vector3(0, 1080f * (1 - timer / duration), 1);
        }
    }
}
