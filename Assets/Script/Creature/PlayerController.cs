using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GlobalEventUtility;

public class PlayerController : CreatureBase
{
    public static PlayerController Instance;
    private PlayerData m_playerStats;
    private Vector3 m_input;
    private float m_acceleration;
    private Rigidbody m_rigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }
    private Rigidbody _rigidbody;
    public bool IsReady => m_playerStats != null;
    public enum PlayerState
    {
        Idle,
        Move,
        Jump,
        Bounce,
        StickOut,
        Dead
    }
    public PlayerState currentState;
    private void Awake()
    {
        SceneDataManager.Instance.DataLoadingIsCompleted += () => m_playerStats = SceneDataManager.Instance.currentPlayerStats;
        Instance = this;
    }
    private void Start()
    {
        m_acceleration = 1;
    }
    private void FixedUpdate()
    {
        if (!IsReady)
            return;

        m_rigidbody.linearVelocity = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsReady)
            return;

        m_input.x = InputUtility.GetInputVector().x;
        m_input.z = InputUtility.GetInputVector().y;

        finalVelocity = Vector3.zero;

        if (!ReachGround(m_playerStats.groundLayer))
        {
            m_acceleration += Time.deltaTime * 2f;
            finalVelocity.y -= 0.981f * m_acceleration * Time.deltaTime;
        }
        else
            m_acceleration = 1;

        //if (InputUtility.IsContactingScreen(out Ray screenRay) && currentState!= PlayerState.Jump && currentState != PlayerState.Bounce)
        //{
        //    if(RaycastUtility.GetHitCounts(screenRay,Mathf.Infinity,LayerMask.GetMask("Player"))>0)
        //    {
        //        OnJump(out float jumpForce);
        //        finalVelocity.y += jumpForce;
        //    }
        //    else if (RaycastUtility.GetHitCounts(screenRay, Mathf.Infinity, LayerMask.GetMask("Enemy"), out Vector3 hitPoint) > 0)
        //    {
        //        OnStickOut(hitPoint);
        //    }
        //}

        //if (HitBounceLayer() && currentState == PlayerState.Jump)
        //{
        //    OnBounce(out float bounceForce);
        //    finalVelocity.y += bounceForce;
        //}

        if (InputUtility.IsInputing())
            OnLookAtFront(m_input);

        if (IsFrontBlocked(m_input))
            finalVelocity.z = 0;
        else
            finalVelocity.z = m_input.magnitude* m_playerStats.moveSpeed * Time.deltaTime;

        OnMove(finalVelocity);
    }

    private void OnJump(out float jumpForce)
    {
        jumpForce = m_playerStats.jumpForce*Time.deltaTime;
        currentState = PlayerState.Jump;
        Debug.Log("Player Jump");
    }

    private void OnBounce(out float bounceForce)
    {
        bounceForce = m_playerStats.bounceForce * Time.deltaTime;
        currentState = PlayerState.Bounce;
        Debug.Log("Player OnBounce");
    }

    private void OnStickOut(Vector3 targetPoint)
    {
        currentState = PlayerState.StickOut;
        Debug.Log("Player Stick Out");
    }
    private bool HitBounceLayer()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return RaycastUtility.GetHitCounts(ray, (m_boxCol.bounds.size.y / 2) + 0.05f, LayerMask.GetMask("Bounce")) > 0;
    }
    private bool IsFrontBlocked(Vector3 dir)
    {
        return RaycastUtility.GetHitCounts(
            new Ray(transform.position, dir), 0.5f, LayerMask.GetMask("Block")) > 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Player Hit Enemy");
            OnPlayerDead();
        }
    }

    private void OnPlayerDead()
    {
        GameOverNotification?.Invoke();
    }
}
