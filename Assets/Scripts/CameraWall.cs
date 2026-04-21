using System.Collections.Generic;
using UnityEngine;

public class CameraWall: MonoBehaviour
{
    public Transform player;

    [SerializeField] LayerMask wallMask;

    [Header("Ray Offsets")]
    [SerializeField] float sideOffset = 1f;
    [SerializeField] float topOffset = 1f;

    private HashSet<MeshRenderer> hiddenRenderers = new HashSet<MeshRenderer>();

    void LateUpdate()
    {
        // Re-enable all renderers from last frame
        foreach (var r in hiddenRenderers)
        {
            if (r)
                r.enabled = true;
        }

        hiddenRenderers.Clear();

        Vector3 camPos = transform.position;
        Vector3 playerPos = player.position;

        Vector3 right = transform.right;

        CastRay(camPos, playerPos);
        CastRay(camPos, playerPos + right * sideOffset);
        CastRay(camPos, playerPos - right * sideOffset);
        CastRay(camPos, playerPos + Vector3.up * topOffset);
    }

    void CastRay(Vector3 origin, Vector3 target)
    {
        Vector3 dir = target - origin;
        float dist = dir.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(origin, dir.normalized, dist, wallMask);

        foreach (var hit in hits)
        {
            MeshRenderer mr = hit.collider.GetComponent<MeshRenderer>();

            if (mr && mr.enabled)
            {
                mr.enabled = false;
                hiddenRenderers.Add(mr);
            }
        }
    }
}