using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public int defaultValue = 0;
    public string preferenceKey = "(insert key here)";
    public Toggle toggle;
    public GameObject element;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int toggleState = PlayerPrefs.GetInt(preferenceKey, defaultValue);
        toggle.isOn = (toggleState == 1);
        if (element != null)
        {
            element.SetActive(toggle.isOn);
        }
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnToggleValueChanged(bool newValue)
    {
        PlayerPrefs.SetInt(preferenceKey, newValue ? 1 : 0);
        PlayerPrefs.Save();
        if (element != null)
        {
            element.SetActive(newValue);
        }
    }
}
