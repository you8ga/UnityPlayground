using UnityEngine;

public class RaycastUtility : MonoBehaviour
{
    private static RaycastHit[] _sharedRaycastHits = new RaycastHit[5];
    /// <summary>
    /// Shared RaycastHit counts
    /// </summary>
    public static int GetHitCounts(Ray ray, float maxDistance, int layerMask)
    {
        return Physics.RaycastNonAlloc(ray, _sharedRaycastHits, maxDistance, layerMask);
    }

    public static int GetHitCounts(Ray ray, float maxDistance, int layerMask,out Vector3 hitPoint)
    {
        int count = Physics.RaycastNonAlloc(ray, _sharedRaycastHits, maxDistance, layerMask);

        if(count > 0)
            hitPoint = _sharedRaycastHits[0].point;
        else
            hitPoint = default;

        return count;
    }
    public static void ClearSharedRaycastHit()
    {
        for (int i = 0; i < _sharedRaycastHits.Length; i++)
        {
            _sharedRaycastHits[i] = default;
        }
    }
    

}
