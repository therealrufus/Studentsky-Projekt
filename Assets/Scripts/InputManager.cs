using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static KeyCode Jump = KeyCode.Space;
    public static KeyCode AbPrimary = KeyCode.Q;
    public static KeyCode AbSecondary = KeyCode.F;
    public static KeyCode Crouch = KeyCode.LeftControl;
    public static KeyCode Action = KeyCode.E;
    public static float mouseSensitivity = 400;

    public static Vector3 GetRawArrowInput()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    public static Vector2 GetMouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity;
    }
}