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
    }

    IEnumerator Grappling()
    {
        //proc jsem nepouzil onMove??

        line.enabled = true;
        while (true)
        {
            Quaternion lastRotation = master.handLeft.rotation;
            master.handLeft.LookAt(grapple.impactPoint);
            Quaternion newRotation = master.handLeft.rotation;

            master.RotateWithEase(master.handLeft, newRotation, lastRotation);

            line.SetPosition(0, master.handLeft.position);
            line.SetPosition(1, grapple.impactPoint);
            yield return null;
        }
    }
}
