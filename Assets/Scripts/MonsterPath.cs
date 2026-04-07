using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class MonsterPath : MonoBehaviour
{
    [SerializeField] PlayerMove PlayerMove;
    [SerializeField] HeartbeatUI heartbeat;
    [SerializeField] GridManager grid;
    [SerializeField] int moveInterval = 3;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Jumpscare jumpscare;
    private (int, int) position = (-2, 2);
    private (int, int) target;
    private int count = 0;
    private bool start = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = PlayerMove.GetPreviousPosition();
        // grid = GridManager.gridObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (heartbeat.getCount() % moveInterval == moveInterval - 1)
            {
                spriteTransform.localPosition = Random.insideUnitSphere * 0.1f;
            }
            else
            {
                spriteTransform.localPosition = Random.insideUnitSphere * 0.01f;
            }
        }
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
        if (heartbeat.getCount() % moveInterval == 0 && count != heartbeat.getCount() && !PlayerMove.getMonsterWait())
        {
            count = heartbeat.getCount();
            if (position == PlayerMove.GetPreviousPosition())
            {
                target = PlayerMove.GetCurrentPosition();
            }
            else
            {
                target = PlayerMove.GetPreviousPosition();
            }
            (int, int) distance = (target.Item1 - position.Item1, target.Item2 - position.Item2);
            if (Mathf.Abs(distance.Item1) >= Mathf.Abs(distance.Item2))
            {
                if (grid.GetTile(position.Item1 + (int) Mathf.Sign(distance.Item1), position.Item2) == "floor")
                {
                    Vector3 targetPos = new Vector3(position.Item1 + Mathf.Sign(distance.Item1), transform.position.y, position.Item2);
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.5f);
                    position = ((int) targetPos.x, (int) targetPos.z);
                }
                else if (grid.GetTile(position.Item1, position.Item2 + (int)Mathf.Sign(distance.Item2)) == "floor")
                {
                    Vector3 targetPos = new Vector3(position.Item1, transform.position.y, position.Item2 + Mathf.Sign(distance.Item2));
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.5f);
                    position = ((int)targetPos.x, (int)targetPos.z);
                }
                else if (grid.GetTile(position.Item1, position.Item2 - (int)Mathf.Sign(distance.Item2)) == "floor")
                {
                    Vector3 targetPos = new Vector3(position.Item1, transform.position.y, position.Item2 + Mathf.Sign(distance.Item2));
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.5f);
                    position = ((int)targetPos.x, (int)targetPos.z);
                }
            }
            else
            {
                if (grid.GetTile(position.Item1, position.Item2 + (int)Mathf.Sign(distance.Item2)) == "floor")
                {
                    Vector3 targetPos = new Vector3(position.Item1, transform.position.y, position.Item2 + Mathf.Sign(distance.Item2));
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.5f);
                    position = ((int)targetPos.x, (int)targetPos.z);
                }
                else if (grid.GetTile(position.Item1 + (int)Mathf.Sign(distance.Item1), position.Item2) == "floor")
                {
                    Vector3 targetPos = new Vector3(position.Item1 + Mathf.Sign(distance.Item1), transform.position.y, position.Item2);
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.5f);
                    position = ((int)targetPos.x, (int)targetPos.z);
                }
                else if (grid.GetTile(position.Item1 - (int)Mathf.Sign(distance.Item1), position.Item2) == "floor")
                {
                    Vector3 targetPos = new Vector3(position.Item1 + Mathf.Sign(distance.Item1), transform.position.y, position.Item2);
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, 2.5f);
                    position = ((int)targetPos.x, (int)targetPos.z);
                }
            }
        }
        StartCoroutine(Chase());
    }

}
