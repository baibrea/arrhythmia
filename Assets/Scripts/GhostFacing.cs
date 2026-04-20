using UnityEngine;

public class GhostFacing : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}