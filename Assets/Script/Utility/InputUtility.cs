using System.Drawing;
using UnityEngine;

public static class InputUtility
{
    private static Vector2 _inputVector;
    private static Ray _contactRay;
    private static RaycastHit[] _pointRaycastHit = new RaycastHit[1];

    public static bool IsKeyPressed(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public static bool IsKeyHeld(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public static bool IsKeyReleased(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    public static Vector2 GetInputVector()
    {
        _inputVector.x = Input.GetAxis("Horizontal");
        _inputVector.y = Input.GetAxis("Vertical");

        return _inputVector;
    }

    public static bool IsInputing()
    {
        return _inputVector != Vector2.zero;
    }

    public static bool IsContactingScreen(out Ray screenRay)
    {
        screenRay = default;
#if (UNITY_STANDALONE || UNITY_EDITOR)
        if (Input.GetMouseButtonDown(0))
        {
            screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            return true;
        }
        else
            return false;

#elif (UNITY_IOS || UNITY_ANDROID)
                if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if(touch.phase != TouchPhase.Began) 
                    continue;

                screenRay = Camera.main.ScreenPointToRay(touch.position);
                return true;
            }
        }
        else
            return false;
#endif
    }

    public static Vector3 GetContactPoint(int validLayer)
    {
        int hits = Physics.RaycastNonAlloc(_contactRay, _pointRaycastHit, Mathf.Infinity, validLayer);
        return _pointRaycastHit[0].point;
    }

}
