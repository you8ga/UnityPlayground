using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target;
    public Vector3 posOffset;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.Instance.transform;
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + posOffset;
    }
}
