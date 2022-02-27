using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public PlayerMovement player;
    public Objective[] objectives;
    [HideInInspector] public UnityEvent onNewObjective;
    public float minDistance = 10;
    [HideInInspector]
    public Objective currentObjective
    {
        get { return _currentObjective; }
        set { _currentObjective = value; onNewObjective.Invoke();}
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

    void AssignNewObjective()
    {
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
        if (objective == currentObjective) AssignNewObjective();
    }
}
