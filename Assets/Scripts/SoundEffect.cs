using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound_Effect", menuName = "new Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public SoundType type;
    public AudioClip[] clips;
    [Space]
    public Vector2 pitch = new Vector2(0.8f, 1.2f);
    public Vector2 volume = new Vector2(1f, 1f);
    public bool loop = false;
}

public enum SoundType 
{
    sound,
    music
}
