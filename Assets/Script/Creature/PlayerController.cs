using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : CreatureBase
{
    private PlayerStats m_playerStats;
    private Vector3 m_input;

    public bool IsReady => m_playerStats != null;
    private void Start()
    {
        LevelManager.Instance.CompleteLoading += () => m_playerStats = LevelManager.Instance.currentPlayerStats;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsReady)
            return;

        m_input.x = InputUtility.GetInputVector().x;
        m_input.z = InputUtility.GetInputVector().y;
        NormalMove(m_input);
    }

    public override void NormalMove(Vector3 dir)
    {
        if (InputUtility.IsInputing())
            transform.LookAt(transform.position + dir);
        transform.Translate(Vector3.forward * dir.magnitude * m_playerStats.moveSpeed * Time.deltaTime);
    }

    private void CollisionDetect(Vector3 dir)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Player Hit Enemy");
        }
    }

}
