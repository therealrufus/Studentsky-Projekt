using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstLevel : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Text text;
    int rings = 0;
    float t;

    private void Start()
    {
        t = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (rings > 0) t += Time.deltaTime;
        if (text != null) text.text = $"{(float)(Mathf.RoundToInt(t * 10f)) / 10f}s | rings left:{rings}";
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Subscribe()
    {
        rings++;
    }

    public void Check()
    {
        rings--;
    }
}
