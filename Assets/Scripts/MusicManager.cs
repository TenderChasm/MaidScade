using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioClip Intro;
    public AudioClip MainroomsBeforeGun;
    public AudioClip MainroomsAfterGun;
    public AudioClip MainroomsChambers;
    public AudioClip Flanny;
    public AudioClip Whitesocks;
    public AudioClip Outro;

    public float DefaultVolume = 0.5F;

    private AudioSource Audio { get; set; }

    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Audio.volume = DefaultVolume;
    }

    public void Play(AudioClip clip)
    {
        Audio.volume = DefaultVolume;
        Audio.clip = clip;
        Audio.Play();
    }

    public void Stop()
    {
        Audio.Stop();
    }

    public void Fade()
    {
        StartCoroutine(FadeCoroutine());
    }

    public void FadeAndPlay(AudioClip clip)
    {
        StartCoroutine(FadeAndPlayCoroutine(clip));
    }

    private IEnumerator FadeAndPlayCoroutine(AudioClip clip)
    {
        yield return StartCoroutine(FadeCoroutine());
        Play(clip);
    }

    private IEnumerator FadeCoroutine()
    {
        for(float volume = Audio.volume; volume > 0; volume-= 0.02F)
        {
            Audio.volume = volume;
            yield return new WaitForSecondsRealtime(0.05F);
        }

        Stop();
    }
}
