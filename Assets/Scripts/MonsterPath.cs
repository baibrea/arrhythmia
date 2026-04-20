using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalWorlds.StarterPackage3D;

public class MonsterPath : MonoBehaviour
{
    [SerializeField] PlayerMove PlayerMove;
    [SerializeField] HeartbeatUI heartbeat;
    [SerializeField] GridManager grid;

    [SerializeField] int moveInterval = 3;

    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Jumpscare jumpscare;

    [Header("Vision")]
    [SerializeField] Transform playerTransform;
    [SerializeField] LayerMask sightMask;
    [SerializeField] float sightDistance = 20f;

    private (int, int) position = (-2, 2);
    private (int, int) lastSeenPlayerPos;

    private int count = 0;
    private bool start = false;

    private List<(int, int)> currentPath;

    class Node
    {
        public (int, int) pos;
        public Node parent;
        public int g;
        public int h;
        public int f => g + h;

        public Node((int, int) p, Node parent, int g, int h)
        {
            pos = p;
            this.parent = parent;
            this.g = g;
            this.h = h;
        }
    }

    void Start()
    {
        lastSeenPlayerPos = PlayerMove.GetPreviousPosition();
    }

    public void AlertMonster()
    {
        lastSeenPlayerPos = PlayerMove.GetCurrentPosition();
        currentPath = null;
    }

    void Update()
    {
        if (!start) return;

        if (heartbeat.getCount() % moveInterval == moveInterval - 1)
            spriteTransform.localPosition = Random.insideUnitSphere * 0.1f;
        else
            spriteTransform.localPosition = Random.insideUnitSphere * 0.01f;
    }

    public void BeginChase()
    {
        start = true;
        StartCoroutine(Chase());
    }

    IEnumerator Chase()
    {
        yield return null;

        if (position == PlayerMove.GetCurrentPosition())
        {
            jumpscare.Scare();
            yield return new WaitForSeconds(5);
        }

        if (heartbeat.getCount() % moveInterval == 0 &&
            count != heartbeat.getCount() &&
            !PlayerMove.getMonsterWait())
        {
            count = heartbeat.getCount();

            if (CanSeePlayer())
            {
                lastSeenPlayerPos = PlayerMove.GetCurrentPosition();
            }

            if (position == lastSeenPlayerPos)
            {
                if (!CanSeePlayer())
                {
                    lastSeenPlayerPos = GetRandomFloorTile();
                }
            }

            currentPath = FindPath(position, lastSeenPlayerPos);

            if (currentPath != null && currentPath.Count > 1)
            {
                var next = currentPath[1];

                Vector3 targetPos = new Vector3(
                    next.Item1,
                    transform.position.y,
                    next.Item2
                );

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    2.5f
                );

                position = next;
            }
        }

        StartCoroutine(Chase());
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 dir = (playerTransform.position - origin).normalized;

        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, sightDistance, sightMask))
        {
            if (hit.transform == playerTransform)
                if (PlayerMove.checkCloset())
                {
                    return false;
                }
                return true;
        }

        return false;
    }

    bool IsWalkable((int, int) pos)
    {
        string tile = grid.GetTile(pos.Item1, pos.Item2);

        if (tile == "floor")
            return true;

        return false;
    }

    List<(int, int)> GetNeighbors((int, int) pos)
    {
        return new List<(int, int)>
        {
            (pos.Item1 + 1, pos.Item2),
            (pos.Item1 - 1, pos.Item2),
            (pos.Item1, pos.Item2 + 1),
            (pos.Item1, pos.Item2 - 1)
        };
    }

    int Heuristic((int, int) a, (int, int) b)
    {
        return Mathf.Abs(a.Item1 - b.Item1) + Mathf.Abs(a.Item2 - b.Item2);
    }

    List<(int, int)> FindPath((int, int) start, (int, int) goal)
    {
        List<Node> open = new List<Node>();
        HashSet<(int, int)> closed = new HashSet<(int, int)>();

        open.Add(new Node(start, null, 0, Heuristic(start, goal)));

        while (open.Count > 0)
        {
            Node current = open[0];

            foreach (var n in open)
                if (n.f < current.f)
                    current = n;

            open.Remove(current);
            closed.Add(current.pos);

            if (current.pos == goal)
                return Reconstruct(current);

            foreach (var neighbor in GetNeighbors(current.pos))
            {
                if (closed.Contains(neighbor))
                    continue;

                if (!IsWalkable(neighbor))
                    continue;

                int g = current.g + 1;

                Node existing = open.Find(n => n.pos == neighbor);

                if (existing == null)
                {
                    open.Add(new Node(
                        neighbor,
                        current,
                        g,
                        Heuristic(neighbor, goal)
                    ));
                }
                else if (g < existing.g)
                {
                    existing.g = g;
                    existing.parent = current;
                }
            }
        }

        return null;
    }

    List<(int, int)> Reconstruct(Node node)
    {
        List<(int, int)> path = new List<(int, int)>();

        while (node != null)
        {
            path.Add(node.pos);
            node = node.parent;
        }

        path.Reverse();
        return path;
    }

    (int, int) GetRandomFloorTile()
    {
        for (int i = 0; i < 50; i++)
        {
            int x = Random.Range(-20, 20);
            int y = Random.Range(-20, 20);

            if (grid.GetTile(x, y) == "floor")
                return (x, y);
        }

        return position;
    }
}