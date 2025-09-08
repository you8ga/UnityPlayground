using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : CreatureBase
{
    private PlayerStats m_playerStats;
    private Vector3 m_input;
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


    private void Awake()
    {
        LevelManager.Instance.CompleteLoading += () => m_playerStats = LevelManager.Instance.currentPlayerStats;
    }
    private void Start()
    {

    }
    private void FixedUpdate()
    {
        if (!IsReady)
            return;

        m_rigidbody.velocity = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsReady)
            return;

        m_input.x = InputUtility.GetInputVector().x;
        m_input.z = InputUtility.GetInputVector().y;
        if (!IsBlocked(m_input))
        {
            NormalMove(m_input);
        }

    }

    public override void NormalMove(Vector3 dir)
    {
        if (InputUtility.IsInputing())
            transform.LookAt(transform.position + dir);
        transform.Translate(Vector3.forward * dir.magnitude * m_playerStats.moveSpeed * Time.deltaTime);
    }

    private bool IsBlocked(Vector3 dir)
    {
        return RaycastUtility.GetSharedRaycastHits(
            new Ray(transform.position, dir), 1f, LayerMask.GetMask("Block")) > 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    Debug.Log("Player Hit Enemy");
        //    OnHit?.Invoke();
        //}
    }

}
