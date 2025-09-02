using UnityEngine;

public static class InputUtility
{
    private static Vector2 _inputVector;

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
}
