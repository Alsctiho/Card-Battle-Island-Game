using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    // public bool persistent = false;

    [HideInInspector]
    public bool isFading = false;
    [HideInInspector]
    public Coroutine coroutine;

}
