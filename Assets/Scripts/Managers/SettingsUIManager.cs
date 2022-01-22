using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public SettingsManager manager;
    public GameObject settingsUI;
    bool on = false;
    public Scrollbar fovBar;
    public Scrollbar sensitivityBar;

    private void Start()
    {
        on = true;
        ToggleSettingsUI();
        fovBar.onValueChanged.AddListener((float val) => manager.SetFOV(val));
        sensitivityBar.onValueChanged.AddListener((float val) => manager.SetSensitivity(val));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsUI();
        }
    }

    void ToggleSettingsUI()
    {
        if (!on)
        {
            Cursor.lockState = CursorLockMode.None;
            settingsUI.SetActive(true);
            on = true;
        }
        else
        {
            ExitUI();
        }
    }

    public void ExitUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        settingsUI.SetActive(false);
        on = false;
    }

    public void LeaveApp()
    {
        Application.Quit();
    }
}
