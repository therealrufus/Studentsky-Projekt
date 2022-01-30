using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimWallrun : PlayerEffect
{
    [Space]
    public MoveWallride wallride;
    Transform target;

    [SerializeField] float angle;
    [SerializeField] float time;

    [SerializeField] Vector3 handOffset;

    [SerializeField] SoundEffect jumpSFX;

    float finalAngle;

    private void Start()
    {
        wallride.OnBegin.AddListener(RotateCam);
        wallride.OnEnd.AddListener(ResetCam);
        wallride.OnMove.AddListener(AnimateHand);
        wallride.OnJump.AddListener(OnJump);
        target = SpawnHolder(master.cam.transform);
    }

    void RotateCam()
    {
        finalAngle = angle * AngleDir(wallride.transform.forward, wallride.normal, Vector3.up) * -1;

        target.DOLocalRotate(new Vector3(0, 0, finalAngle), time);
    }

    void ResetCam()
    {
        target.DOLocalRotate(new Vector3(0, 0, 0), time);
    }

    void AnimateHand()
    {
        float lookDir = AngleDir(wallride.transform.forward, wallride.normal, Vector3.up) * -1;
        Vector3 lookVector = Vector3.Cross(wallride.transform.up, wallride.normal) * lookDir;
        Vector3 finalHandPos = wallride.transform.position;
        finalHandPos += lookVector * handOffset.x;
        finalHandPos += Vector3.up * handOffset.y;
        finalHandPos += wallride.normal * handOffset.z;

        master.MoveWithEase(lookDir == 1 ? master.handRight : master.handLeft, finalHandPos, false, 0.2f);
        master.RotateWithEase(lookDir == 1 ? master.handRight : master.handLeft, Quaternion.identity);

        Debug.DrawRay(wallride.transform.position, lookVector, Color.black);

    }

    void OnJump()
    {
        SoundManager.Play(jumpSFX);
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }

        return 0f;
    }
}
