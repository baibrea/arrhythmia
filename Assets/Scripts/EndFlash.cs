using UnityEngine;

public class EndFlash : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Go()
    {
        flashlight.End();
    }

}
