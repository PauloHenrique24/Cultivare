using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager current;
    private AudioSource audio_;

    public List<AudioClip> clips;

    void Awake()
    {
        if (current == null)
            current = this;

        audio_ = GetComponent<AudioSource>();
    }

    public void PlaySound(bool loop,AudioClip clip,float volume)
    {
        audio_.clip = clip;
        audio_.loop = loop;
        audio_.volume = volume;

        audio_.Play();
    }

    public void PauseSound()
    {
        audio_.Pause();
    }
}
