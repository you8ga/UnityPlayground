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

    private void InitStats()
    {
        transform.position = TilemapController.Instance.RandomMapXZVector();
        m_stats = LevelManager.Instance.currentDarkGStats;
    }
    private void Awake()
    {
        LevelManager.Instance.CompleteLoading += () => InitStats();
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
                targetVector = TilemapController.Instance.RandomMapXZVector(transform.position.y);
                break;
            case GState.Move:
                NormalMove(GetMoveDirection());
                break;
            case GState.Attack:
                break;
            case GState.Die:
                break;
            default:
                break;
        }
    }

    protected Vector3 GetMoveDirection()
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
