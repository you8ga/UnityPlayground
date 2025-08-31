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
        m_stats = await AddressableManager.Instance.LoadAssetAsync<PlayerStats>(AddressableKey.PlayerStats);
    }

    // Update is called once per frame
    public void Update()
    {
        if (m_stats == null)
            return;

        m_input.x = InputManager.Instance.GetInputVector().x;
        m_input.z = InputManager.Instance.GetInputVector().y;
        NormalMove(m_input);
    }

    public override void NormalMove(Vector3 dir)
    {
        if (InputManager.Instance.IsInputing())
            transform.LookAt(transform.position + dir);
        transform.Translate(Vector3.forward * dir.magnitude * m_stats.moveSpeed * Time.deltaTime);

    }
}
