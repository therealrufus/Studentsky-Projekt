using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimGrapple : PlayerEffect
{
    public MoveGrapple grapple;
    public LineRenderer line;

    Coroutine grapplingCoroutine;

    void Start()
    {
        grapple.OnBegin.AddListener(StartGrappling);
        grapple.OnEnd.AddListener(StopGrappling);
        line.enabled = false;
    }

    void StartGrappling()
    {
        grapplingCoroutine = StartCoroutine("Grappling");
    }
    void StopGrappling() 
    {
        StopCoroutine(grapplingCoroutine);
        line.enabled = false;
        master.handLeft.rotation = Quaternion.identity;
    }

    IEnumerator Grappling()
    {
        line.enabled = true;
        while (true)
        {
            master.handLeft.LookAt(grapple.impactPoint);
            line.SetPosition(0, master.handLeft.position);
            line.SetPosition(1, grapple.impactPoint);
            yield return null;
            //yield return null;
        }
    }
}
