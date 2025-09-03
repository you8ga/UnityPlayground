using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkG : CreatureBase
{
    protected DarkGStats m_stats;
    protected Vector3 targetVector;
    protected bool isReachingTarget => Vector3.Distance(transform.position, targetVector) <= m_stats.reachDistance;
    public bool isReady => m_stats != null;
    public enum GState
    {
        Idle,
        Decide,
        Move,
        Attack,
        Die
    }
    private GState m_state = GState.Idle;
    private void Awake()
    {
        LevelManager.Instance.CompleteLoading += () => m_stats = LevelManager.Instance.currentDarkGStats;
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (!isReady)
            return;

        CheckStateConditions();
        StateAction(m_state);
    }
    public override void NormalMove(Vector3 dir)
    {
        transform.LookAt(transform.position + dir);
        transform.Translate(Vector3.forward * dir.magnitude * m_stats.moveSpeed * Time.deltaTime);
    }

    private void CheckStateConditions()
    {
        if (m_state == GState.Idle)
            ChangeState(GState.Decide);
        else if (isReachingTarget)
            ChangeState(GState.Idle);
        else if(m_state == GState.Decide)
            ChangeState(GState.Move);
    }

    private void ChangeState(GState newState)
    {
        if (m_state == newState)
            return;

        Debug.Log($"(frame = [{Time.frameCount}]) Change [{gameObject.name}] State from [{m_state}] to [{newState}].");
        m_state = newState;
    }

    private void StateAction(GState currentState)
    {
        switch (currentState)
        {
            case GState.Idle:
                break;
            case GState.Decide:
                targetVector = RandomMapVector();
                break;
            case GState.Move:
                NormalMove(GetDirection());
                break;
            case GState.Attack:
                break;
            case GState.Die:
                break;
            default:
                break;
        }
    }

    protected Vector3 RandomMapVector()
    {
        float xRange = Mathf.Round(TilemapController.Instance.maxMapArea.x);
        float zRange = Mathf.Round(TilemapController.Instance.maxMapArea.z);

        Vector3 randomPos = new Vector3(Random.Range(-xRange / 2, xRange / 2),transform.position.y,Random.Range(-zRange / 2, zRange / 2));
        return randomPos;
    }

    protected Vector3 GetDirection()
    {
        Vector3 dir = (targetVector - transform.position).normalized;
        return dir;
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetVector, 0.2f);
        Gizmos.DrawLine(transform.position, targetVector);
    }

}
