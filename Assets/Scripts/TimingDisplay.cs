using TMPro;
using UnityEngine;

public class TimingDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.color.a == 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

}
