using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] Audioclip;
    private AudioSource Audiosource;

    void Start()
    {
        Audiosource = GetComponent<AudioSource>();
    }

    public void PlaySound(int num)
    {
        Audiosource.PlayOneShot(Audioclip[num]);
    }

    public void SoundP()
    {
        Audiosource.Pause();
    }

    public void SoundUnP()
    {
        Audiosource.UnPause();
    }
}

