using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public PlayerMovement player;
    public Objective[] objectives;
    public float minDistance = 10;
    [Header("score")]
    //https://www.desmos.com/calculator/fnvmmxu2vi
    public float curveSteepness = 150;
    public float curveStartTime = 30;
    float timer;
    int score;
    public Text text;

    [HideInInspector] public UnityEvent onNewObjective;
    [HideInInspector]
    public Objective currentObjective
    {
        get { return _currentObjective; }
        set { _currentObjective = value; onNewObjective.Invoke(); }
    }

    Objective _currentObjective;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //we should pre-compute this
        objectives = GameObject.FindObjectsOfType<Objective>();
        AssignNewObjective();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameOver();
        }
        text.text = Mathf.Round(timer).ToString();
    }

    void AssignNewObjective()
    {
        score++;
        timer = GetTime(score);
        
        Objective newObjective = null;

        for (int i = 0; i < 500; i++)
        {
            newObjective = objectives[Random.Range(0, objectives.Length)];
            if (Vector2.Distance(player.transform.position, newObjective.transform.position) > minDistance)
                break;
        }
        currentObjective = newObjective;
    }

    public void PassedThroughObjective(Objective objective)
    {
        if (objective == currentObjective) 
        {
            AssignNewObjective();
        }
    }

    float GetTime(float x)
    {
        float value = curveSteepness / (x + (curveSteepness / curveStartTime));
        return value;
    }

    void GameOver()
    {

    }
}
