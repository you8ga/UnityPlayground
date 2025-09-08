using System;
using System.Threading.Tasks;
using UnityEngine;

public class CreatureBase : MonoBehaviour,IHitable, IAttackable
{
    public virtual void NormalMove(Vector3 dir)
    {

    }

    public Action OnHit { get; set; }
    public Action OnAttack { get; set; }
}
