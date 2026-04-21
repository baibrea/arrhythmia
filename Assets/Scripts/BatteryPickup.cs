using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [SerializeField] Flashlight flashlight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flashlight.Recharge();
            Destroy(gameObject);
        }
    }

}
