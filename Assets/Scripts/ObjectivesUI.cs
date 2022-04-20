using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{
    public Image img;
    Transform target;
    public Transform cam;
    public GameObject gameOverUI;
    public Text scoreText;

    private void Start()
    {
        ObjectiveManager.instance.onNewObjective.AddListener(ChangeObjective);
        target = ObjectiveManager.instance.currentObjective.transform;

        ObjectiveManager.instance.onGameOver.AddListener(GameOver);
        gameOverUI.SetActive(false);
    }

    void ChangeObjective()
    {
        target = ObjectiveManager.instance.currentObjective.transform;
    }

    private void Update()
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position);

        if (Vector3.Dot((target.position - cam.position), cam.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("ActualGame");
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        scoreText.text = ObjectiveManager.instance.score.ToString();
    }
}
