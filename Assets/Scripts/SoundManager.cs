using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager LocalInstance;
    public List<AudioMixerGroup> mixers;

    public List<AudioSource> Pool;
    public List<AudioSource> InUse;
    [SerializeField] int poolNumber = 10;

    private void Awake()
    {
        LocalInstance = this;
        Inicialize();
    }

    private void Update()
    {
        CheckClips();
    }

    void Inicialize()
    {
        Pool = new List<AudioSource>();

        for (int i = 0; i < poolNumber; i++)
        {
            GameObject audioHolder = new GameObject("Pool num: " + i);
            audioHolder.transform.parent = transform;
            AudioSource source = audioHolder.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0;

            Pool.Add(source);
        }
    }

    public static AudioSource Play(SoundEffect effect)
    {
        if (LocalInstance != null)
        {
            return LocalInstance.PlayLocal(effect);
        }
        else
        {
            Debug.LogError("No Instance detected!");
            return null;
        }
    }

    AudioSource PlayLocal(SoundEffect effect)
    {
        if (Pool.Count > 0)
        {
            AudioSource source = Pool[1];
            Pool.Remove(source);
            InUse.Add(source);

            InitAudioSource(effect, source);

            source.Play();

            return source;
        }
        else
        {
            Debug.LogError("Not enough audioSources in pool!");
            return null;
        }
    }

    void InitAudioSource(SoundEffect effect, AudioSource source)
    {
        source.clip = effect.clips[Random.Range(0, effect.clips.Length)];
        source.loop = effect.loop;
        source.pitch = Random.Range(effect.pitch.x, effect.pitch.y);
        source.volume = Random.Range(effect.volume.x, effect.volume.y);
        source.outputAudioMixerGroup = mixers[(int)effect.type];
    }

    List<AudioSource> queue;
    void CheckClips()
    {
        queue = new List<AudioSource>();

        foreach (var item in InUse)
        {
            if (!item.isPlaying)
            {
                Pool.Add(item);
                queue.Add(item);
            }
        }

        foreach (var item in queue)
        {
            InUse.Remove(item);
        }
    }
}
