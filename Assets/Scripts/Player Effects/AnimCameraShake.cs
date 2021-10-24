using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCameraShake : PlayerEffect
{
    //https://www.youtube.com/watch?v=tu-Qe66AvtY&t=1385s&ab_channel=GDC

    Transform target;
    [Range(0f, 1f)]
    public float shake;
    public float speed;
    [Space]
    public float maxYaw;
    public float maxPitch;
    public float maxRoll;
    [Space]
    public Vector2 minSpeed;

    private void Start()
    {
        target = SpawnHolder(master.cam.transform);
    }

    private void Update()
    {
        shake = Mathf.InverseLerp(minSpeed.x, minSpeed.y, master.playerMovement.SPEED.magnitude);
        Shake(69);
    }

    void Shake(float seed)
    {
        float yaw = maxYaw * shake * GetPerlinNoise(seed, Time.time * speed);
        float pitch = maxPitch * shake * GetPerlinNoise(seed + 1, Time.time * speed);
        float roll = maxRoll * shake * GetPerlinNoise(seed + 2, Time.time * speed);
        target.transform.localRotation = Quaternion.Euler(new Vector3(pitch, yaw, roll));
    }

    float GetPerlinNoise(float seed, float time)
    {
        return (Mathf.PerlinNoise(seed, time) * 2f) - 1f;
    }
}
