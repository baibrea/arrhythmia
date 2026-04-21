using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public int defaultValue = 0;
    public string preferenceKey = "(insert key here)";
    public Toggle toggle;
    public GameObject[] elements;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int toggleState = PlayerPrefs.GetInt(preferenceKey, defaultValue);
        toggle.isOn = (toggleState == 1);
        foreach (GameObject element in elements)
        {
            if (element != null)
            {
                element.SetActive(toggle.isOn);
            }
        }
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnToggleValueChanged(bool newValue)
    {
        PlayerPrefs.SetInt(preferenceKey, newValue ? 1 : 0);
        PlayerPrefs.Save();
        foreach (GameObject element in elements)
        {
            if (element != null)
            {
                element.SetActive(newValue);
            }
        }
    }
}
