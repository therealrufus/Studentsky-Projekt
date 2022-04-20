using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Camera playerCam;
    Vector2 minmaxFOV = new Vector2(70, 150);
    public Vector2 minmaxSensitivity = new Vector2(100, 800);

    private void Start()
    {
        InputManager.mouseSensitivity = PlayerPrefs.GetFloat("sensitivity", 400);
        playerCam.fieldOfView = PlayerPrefs.GetFloat("fov", 100);
    }

    public void SetSensitivity(float f)
    {
        InputManager.mouseSensitivity = Mathf.Lerp(minmaxSensitivity.x, minmaxSensitivity.y, f);
        Debug.Log($"f{f} x{minmaxSensitivity.x} y{minmaxSensitivity.y} = {Mathf.Lerp(minmaxSensitivity.x, minmaxSensitivity.y, f)}");
        PlayerPrefs.SetFloat("sensitivity", InputManager.mouseSensitivity);
    }

    public void SetFOV(float f)
    {
        playerCam.fieldOfView = Mathf.Lerp(minmaxFOV.x, minmaxFOV.y, f);
        PlayerPrefs.SetFloat("fov", playerCam.fieldOfView);
    }
}
