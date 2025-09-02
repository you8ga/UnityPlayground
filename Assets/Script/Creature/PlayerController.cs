using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureBase
{
    private PlayerStats m_stats;
    private Vector3 m_input;
    // Start is called before the first frame update
    private async void Start()
    {
        m_stats = await AddressableUtility.LoadAssetAsync<PlayerStats>(AddressableKey.PlayerStats);
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_stats == null)
            return;

        m_input.x = InputUtility.GetInputVector().x;
        m_input.z = InputUtility.GetInputVector().y;
        NormalMove(m_input);
    }

    public override void NormalMove(Vector3 dir)
    {
        if (InputUtility.IsInputing())
            transform.LookAt(transform.position + dir);
        transform.Translate(Vector3.forward * dir.magnitude * m_stats.moveSpeed * Time.deltaTime);
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
