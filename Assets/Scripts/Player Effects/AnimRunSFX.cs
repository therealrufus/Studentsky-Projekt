using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRunSFX : PlayerEffect
{
    [Space]
    [SerializeField] MoveBasic move;
    [SerializeField] SoundEffect effect;
    [SerializeField] float multiplier = 0.1f;
    AudioSource source;

    private void Start()
    {
        move.OnBegin.AddListener(Begin);
        move.OnEnd.AddListener(End);
        move.OnMove.AddListener(Move);
    }

    void Move()
    {
        if (master.playerMovement.grounded)
        {
            if (source != null)
            {
                source.pitch = master.playerMovement.SPEED.magnitude * multiplier;
            }
            else { Begin(); }
        }
        else { End(); }
    }

    void Begin()
    {
        if (master.playerMovement.grounded)
        {
            source = SoundManager.Play(effect);
        }
    }

    void End()
    {
        if (source != null) source.Stop();

        source = null;
    }
}
