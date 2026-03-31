using UnityEngine;

public class DialogueFollow : MonoBehaviour
{
    public Transform target;   // the object to follow (DialogueAnchor)
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        transform.position = screenPos;
    }
}