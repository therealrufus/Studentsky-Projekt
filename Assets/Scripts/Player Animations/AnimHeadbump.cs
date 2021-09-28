using System.Collections;
using UnityEngine;

public class AnimHeadbump : PlayerEffect
{
    public PlayerMoveOption[] moves;
    [Header("headbump")]
    [SerializeField] AnimationCurve headBumpCurve;
    [SerializeField] float headBumpForce;
    [SerializeField] float headBumpSpeed;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = SpawnHolder(master.cam.transform);
        foreach (var item in moves)
        {
            item.OnJump.AddListener(AnimateJump);
            item.OnLand.AddListener(AnimateLanding);
        }
    }

    void AnimateJump()
    {
        headBumpForce = Mathf.Abs(headBumpForce);
        StartCoroutine("JumpAnimation");
    }

    void AnimateLanding()
    {
        headBumpForce = -1 * Mathf.Abs(headBumpForce);
        StartCoroutine("JumpAnimation");
    }

    IEnumerator JumpAnimation()
    {
        float time = 0;
        while (time < 1)
        {
            target.localEulerAngles = new Vector3(headBumpCurve.Evaluate(time) * headBumpForce, 0, 0);
            time += Time.deltaTime * headBumpSpeed;
            yield return null;
        }
    }
}
