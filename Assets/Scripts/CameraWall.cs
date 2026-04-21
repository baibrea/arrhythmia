using System.Collections.Generic;
using UnityEngine;

public class CameraWall : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask wallMask;

    private HashSet<Renderer> currentlyHidden = new HashSet<Renderer>();
    private HashSet<Renderer> nextHidden = new HashSet<Renderer>();

    void LateUpdate()
    {
        nextHidden.Clear();

        Vector3 camPos = transform.position;
        Vector3 playerPos = player.position;

        Vector3 dir = playerPos - camPos;
        float dist = dir.magnitude;

        Ray ray = new Ray(camPos, dir.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, dist, wallMask);

        foreach (var hit in hits)
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (r != null)
                nextHidden.Add(r);
        }

        foreach (var r in nextHidden)
        {
            if (!currentlyHidden.Contains(r))
            {
                SetVisible(r, false);
            }
        }
        foreach (var r in currentlyHidden)
        {
            if (!nextHidden.Contains(r))
            {
                SetVisible(r, true);
            }
        }

        var temp = currentlyHidden;
        currentlyHidden = nextHidden;
        nextHidden = temp;
    }

    void SetVisible(Renderer r, bool visible)
    {
        foreach (var mat in r.materials)
        {
            Color c = mat.color;
            c.a = visible ? 1f : 0.4f;
            mat.color = c;
        }
    }
}