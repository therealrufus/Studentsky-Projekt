using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public SettingsManager manager;
    public GameObject settingsUI;
    public Scrollbar fovBar;
    public Scrollbar sensitivityBar;

    private void Start()
    {
        sensitivityBar.value = Mathf.InverseLerp(manager.minmaxSensitivity.x, manager.minmaxSensitivity.y, PlayerPrefs.GetFloat("sensitivity", 400));
        //fovBar.onValueChanged.AddListener((float val) => manager.SetFOV(val));
        sensitivityBar.onValueChanged.AddListener((float val) => manager.SetSensitivity(val));
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LeaveApp()
    {
        Application.Quit();
    }
}
