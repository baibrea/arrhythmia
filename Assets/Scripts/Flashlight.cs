using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject flashlight;
    [SerializeField] private Animator animator;
    [SerializeField] private Image batteryImage;
    [SerializeField] private Sprite charge3;
    [SerializeField] private Sprite charge2;
    [SerializeField] private Sprite charge1;
    [SerializeField] private Sprite charge0;

    private int charge = 3;
    private BoxCollider box;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider>();
        box.enabled = false;
        flashlight.SetActive(false);
        batteryImage.sprite = charge3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Flash()
    {
        if (charge > 0)
        {
            box.enabled = true;
            flashlight.SetActive(true);
            animator.SetTrigger("Flash");
            charge -= 1;

            if (charge == 2)
            {
                batteryImage.sprite = charge2;
            }
            else if (charge == 1)
            {
                batteryImage.sprite = charge1;
            }
            else
            {
                batteryImage.sprite = charge0;
            }
        }
    }

    public void Recharge()
    {
        charge = 3;
        batteryImage.sprite = charge3;
    }

    public void End()
    {
        box.enabled = false;
        flashlight.SetActive(false);
    }
}
