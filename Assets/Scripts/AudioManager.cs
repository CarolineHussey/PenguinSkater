using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { return instance; } }
    private static AudioManager instance;

    [SerializeField] private float musicVolume = 1;

    private AudioSource music1;
    private AudioSource music2;
    private AudioSource sfxSource;
    private bool firstMusicSourceActive;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);

        music1 = gameObject.AddComponent<AudioSource>();
        music2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        music1.loop = true;
        music2.loop = true;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    public void PlayMusicWithXFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        //Determine the active audio source
        AudioSource activeSource = (firstMusicSourceActive) ? music1 : music2;
        AudioSource newSource = (firstMusicSourceActive) ? music2 : music1;

        firstMusicSourceActive = !firstMusicSourceActive;

        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithXFade(activeSource, newSource, musicClip, transitionTime));
    }

    private IEnumerator UpdateMusicWithXFade(AudioSource original, AudioSource newSource, AudioClip music, float transitionTime) 
    {
        // Make sure the source is active and playing
        if (!original.isPlaying)
            original.Play();

        newSource.Stop();
        newSource.clip = music;
        newSource.Play();

        float t = 0.0f;

        for (t = 0.0f; t <= transitionTime; t += Time.deltaTime)
        {
            original.volume = musicVolume - ((t / transitionTime) * musicVolume);
            newSource.volume = (t / transitionTime) * musicVolume;
            yield return null;
        }

        original.volume = 0;
        newSource.volume = musicVolume;

        original.Stop();

    }
}
