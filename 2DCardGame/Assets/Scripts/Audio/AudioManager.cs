using UnityEngine.Audio;
using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

    }

    void Start()
    {
        Play("theme");
    }

    public static void Play(string name)
    {
        // Debug.Log($"Try to play sound: {name}");
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume; // Set to initial volume
        s.source.Play();
        s.isFading = false;
        if (s.coroutine != null)
        {
            instance.StopCoroutine(s.coroutine);
            s.coroutine = null;
        }

    }

    public static void FadeOut(string name)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.coroutine = instance.StartCoroutine(FadeOut(s, 0.6f));
    }

    public static void StopPlaying(string name)
    {
        Sound s = Array.Find(instance.sounds, item => item.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    public static void StopAll()
    {
        // instance.StopAllCoroutines();
        foreach (Sound s in instance.sounds)
        {
            // if (s.persistent) continue;
            // s.source.Stop();
            if (s.source.isPlaying && !s.isFading)
            {
                s.coroutine = instance.StartCoroutine(FadeOut(s, 1.0f));
            }
        }
    }

    public static IEnumerator FadeOut(Sound sound, float FadeTime)
    {
        sound.isFading = true;

        float startVolume = sound.source.volume;

        while (sound.source.volume > 0)
        {
            sound.source.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        sound.source.Stop();
        sound.source.volume = startVolume;
        sound.isFading = false;
        sound.coroutine = null;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float FadeTime)
    {
        float _targetVolume = Mathf.Min(1.0f, targetVolume);

        audioSource.Play();

        while (audioSource.volume < _targetVolume)
        {
            audioSource.volume += _targetVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = _targetVolume;
    }
}
