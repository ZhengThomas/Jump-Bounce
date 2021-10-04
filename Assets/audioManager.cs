using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class audioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Sound[] sounds;
    public static float globalVolume = 1;
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void PlaySound(string name)
    {
        var toPlay = Array.Find(sounds, sound => sound.soundId == name);
        toPlay.source.volume = toPlay.volume * globalVolume;
        toPlay.source.Play();
    }

    public void muteUnmute()
    {
        //this is a function because unitys eventhandlers can only work with functions
        //makes sense this way anyways
        if (globalVolume == 0)
        {
            globalVolume = 1;
            print("unmute");
        }
        else if (globalVolume > 0)
        {
            globalVolume = 0;
            print("mute");
        }
        print("asd");
    }
}
