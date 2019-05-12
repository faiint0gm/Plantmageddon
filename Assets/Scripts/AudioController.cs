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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.clip = themeMusic;
        musicSource.Play();
    }
}
