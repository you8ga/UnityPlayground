using System;
using System.Threading.Tasks;
using UnityEngine;

public class CreatureBase : MonoBehaviour,IHitable, IAttackable
{
    public Vector3 finalVelocity;
    protected BoxCollider m_boxCol
    {
        get
        {
            if (_boxCol == null)
                _boxCol = GetComponent<BoxCollider>();
            return _boxCol;
        }
    }
    protected BoxCollider _boxCol;
    public virtual void OnMove(Vector3 velocity)
    {
        transform.Translate(velocity);
    }

    public virtual void OnLookAtFront(Vector3 dir)
    {
        transform.LookAt(transform.position + dir);
    }

    public Action OnHit { get; set; }
    public Action OnAttack { get; set; }

    protected bool ReachGround(LayerMask groundLayer)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return RaycastUtility.GetHitCounts(ray, (m_boxCol.bounds.size.y/2)+0.05f, groundLayer) > 0;
    }
}
