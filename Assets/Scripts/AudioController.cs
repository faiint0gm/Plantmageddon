using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    AudioSource musicSource;
    public AudioClip themeMusic;
    public bool playMusic;
    public AudioMixerGroup musicGroup;
    AudioController[] other;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        other = FindObjectsOfType<AudioController>();
        foreach(AudioController ac in other)
        {
            if(ac != this)
            {
                Destroy(ac.gameObject);
            }
        }
    }
    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.clip = themeMusic;
        musicSource.Play(); 
    }

    public void SetupAudio()
    {
        musicSource.Stop();
        musicSource.clip = null;
        musicSource.loop = true;
        musicSource.clip = themeMusic;
        musicSource.Play();
    }
}
