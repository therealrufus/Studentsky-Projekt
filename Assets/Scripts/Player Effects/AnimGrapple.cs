using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimGrapple : PlayerEffect
{
    [SerializeField] MoveGrapple grapple;
    [SerializeField] LineRenderer line;
    [SerializeField] Image img;

    Coroutine grapplingCoroutine;
    Transform target;

    [SerializeField] float maxAngle;

    void Update()
    {
        img.fillAmount = 1 - grapple.realCooldown / grapple.cooldown;
    }

    void Start()
    {
        grapple.OnBegin.AddListener(StartGrappling);
        grapple.OnEnd.AddListener(StopGrappling);
        grapple.OnMove.AddListener(UpdateGrapple);
        line.enabled = false;
        target = SpawnHolder(master.cam.transform);
    }

    void StartGrappling()
    {
        grapplingCoroutine = StartCoroutine("Grappling");
    }
    void StopGrappling()
    {
        StopCoroutine(grapplingCoroutine);
        line.enabled = false;
        target.DOLocalRotate(Vector3.zero, 0.3f);
    }

    void UpdateGrapple()
    {
        Vector3 camDir = master.camHolder.forward;
        Vector3 pointDir = grapple.direction;
        Vector3 goalDir = Vector3.RotateTowards(camDir.normalized, pointDir.normalized, maxAngle, 10000);

        target.forward = Vector3.Lerp(target.forward, goalDir, master.easeFactor * 0.5f);
    }

    IEnumerator Grappling()
    {
        //proc jsem nepouzil onMove??
        //onMove je nejakej divnej

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
