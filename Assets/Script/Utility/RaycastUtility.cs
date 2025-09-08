using UnityEngine;

public class RaycastUtility : MonoBehaviour
{
    public static RaycastHit[] sharedRaycastHits = new RaycastHit[5];
    public static int GetSharedRaycastHits(Ray ray, float maxDistance, int layerMask)
    {
        return Physics.RaycastNonAlloc(ray, sharedRaycastHits, maxDistance, layerMask);
    }

    public static void ClearSharedRaycastHit()
    {
        for (int i = 0; i < sharedRaycastHits.Length; i++)
        {
            sharedRaycastHits[i] = default;
        }
    }
}
