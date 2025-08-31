using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonBase<InputManager>
{
    private Vector2 m_inputVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsKeyPressed(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public bool IsKeyHeld(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public bool IsKeyReleased(KeyCode key)
    {
        return Input.GetKeyUp(key);
    }

    public Vector2 GetInputVector()
    {
        m_inputVector.x = Input.GetAxis("Horizontal");
        m_inputVector.y = Input.GetAxis("Vertical");

        return m_inputVector;
    }

    public bool IsInputing()
    {
        return m_inputVector != Vector2.zero;
    }
}
