using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEventUtility;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DarkG : CreatureBase
{
    protected DarkGData m_stats;
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
        //SceneDataManager.Instance.DataLoadingIsCompleted += InitStats;
        TilemapController.Instance.OnMapReady += InitStartPosition;
        InitStats();
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
    private void InitStats()
    {
        m_stats = SceneDataManager.Instance.currentDarkGStats;
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

        //Debug.Log($"(frame = [{Time.frameCount}]) Change [{gameObject.name}] State from [{m_state}] to [{newState}].");
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
                var dir = GetMoveDirection();
                OnLookAtFront(dir);
                OnMove(dir.magnitude *Vector3.forward * m_stats.moveSpeed * Time.deltaTime);
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

    private void InitStartPosition()
    {
        Vector3 startPos = TilemapController.Instance.RandomMapXZVector();
        while (TilemapController.Instance.IsStartZone(TilemapController.Instance.GetMapCenter(), startPos, 2f))
        {
            startPos = TilemapController.Instance.RandomMapXZVector();
            //Debug.Log($"ReRandom Pos: {startPos}");
        }
        transform.position = startPos;
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
