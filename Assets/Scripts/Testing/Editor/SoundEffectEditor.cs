using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundEffect))]
public class SoundEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundEffect effect = (SoundEffect)target;

        base.OnInspectorGUI();

        GUILayout.Space(20);

        if (GUILayout.Button("Play"))
        {
            AudioSource source = GameObject.Find("Sound Manager").GetComponent<AudioSource>();

            source.clip = effect.clips[Random.Range(0, effect.clips.Length)];
            source.loop = effect.loop;
            source.pitch = Random.Range(effect.pitch.x, effect.pitch.y);
            source.volume = Random.Range(effect.volume.x, effect.volume.y);

            source.Play();
        }
        if (GUILayout.Button("Play low"))
        {
            AudioSource source = GameObject.Find("Sound Manager").GetComponent<AudioSource>();

            source.clip = effect.clips[Random.Range(0, effect.clips.Length)];
            source.loop = effect.loop;
            source.pitch = effect.pitch.x;
            source.volume = Random.Range(effect.volume.x, effect.volume.y);

            source.Play();
        }
        if (GUILayout.Button("Play high"))
        {
            AudioSource source = GameObject.Find("Sound Manager").GetComponent<AudioSource>();

            source.clip = effect.clips[Random.Range(0, effect.clips.Length)];
            source.loop = effect.loop;
            source.pitch = effect.pitch.y;
            source.volume = Random.Range(effect.volume.x, effect.volume.y);

            source.Play();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Stop"))
        {
            AudioSource source = GameObject.Find("Sound Manager").GetComponent<AudioSource>();

            source.Stop();
        }
    }
}
