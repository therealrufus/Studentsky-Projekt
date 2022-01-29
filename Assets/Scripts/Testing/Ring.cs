using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public FirstLevel levelManager;

    private void Start()
    {
        levelManager.Subscribe();
    }

    private void OnTriggerEnter(Collider other)
    {
        levelManager.Check();
        gameObject.SetActive(false);
    }
}
